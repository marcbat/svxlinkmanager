function DownloadUpdate(id) {
  $('#download-' + id).prop("disabled", true);
};

function UpdateDownloadStatus(id, percent) {
  $('#download-' + id).text("En cours " + percent + "%")
}

function UpdateInstallStatus(id, texte) {
  $('#install-' + id).prop("disabled", true);
  $('#install-' + id).text(texte)
}