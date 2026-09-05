#!/bin/bash
# Hook SessionStart : prepare l'environnement de dev SvxlinkManager
# (SDK .NET, typages TypeScript, paquets NuGet) pour Claude Code on the web.
set -euo pipefail

# Ne s'execute que dans les environnements distants (web / mobile).
if [ "${CLAUDE_CODE_REMOTE:-}" != "true" ]; then
  exit 0
fi

PROJECT_DIR="${CLAUDE_PROJECT_DIR:-$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)}"
WEB_PROJECT="$PROJECT_DIR/src/SvxlinkManager"

export DEBIAN_FRONTEND=noninteractive
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1
# Les projets ciblent net5.0, hors support et absent des depots : le SDK 8.0 sait
# les compiler, et le roll-forward permet de les executer sur le runtime 8.0.
export DOTNET_ROLL_FORWARD=LatestMajor

SUDO=""
if [ "$(id -u)" -ne 0 ]; then
  command -v sudo >/dev/null 2>&1 && SUDO="sudo"
fi

# 1. SDK .NET 8 (idempotent : rien a faire si l'image le fournit deja).
if ! command -v dotnet >/dev/null 2>&1; then
  echo "[session-start] Installation du SDK .NET 8..."
  $SUDO apt-get update -qq
  $SUDO apt-get install -y -qq dotnet-sdk-8.0
fi
echo "[session-start] dotnet $(dotnet --version)"

# 2. Typages TypeScript (@types/jquery, @types/ace, ...) : sans eux la cible
#    Microsoft.TypeScript.MSBuild fait echouer le build du projet web.
if [ -f "$WEB_PROJECT/package.json" ]; then
  echo "[session-start] npm install (typages TypeScript)..."
  (cd "$WEB_PROJECT" && npm install --no-save --no-audit --no-fund --loglevel=error) \
    || echo "[session-start] AVERTISSEMENT : npm install a echoue."
fi

# 3. Paquets NuGet (pre-chauffe le cache, conserve dans l'image mise en cache).
echo "[session-start] dotnet restore..."
dotnet restore "$PROJECT_DIR/SvxlinkManager.sln" \
  || echo "[session-start] AVERTISSEMENT : dotnet restore a echoue."

# 4. Variables exportees pour toute la session Claude.
if [ -n "${CLAUDE_ENV_FILE:-}" ]; then
  {
    echo 'export DOTNET_CLI_TELEMETRY_OPTOUT=1'
    echo 'export DOTNET_NOLOGO=1'
    echo 'export DOTNET_ROLL_FORWARD=LatestMajor'
  } >> "$CLAUDE_ENV_FILE"
fi

echo "[session-start] Environnement pret."
