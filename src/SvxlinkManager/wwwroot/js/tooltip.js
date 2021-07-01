function SetToolTips() {
  $('[data-toggle="tooltip"]').tooltip({
    html: true
  })
}

function SetPopOver() {
  $('[data-toggle="popover"]').popover()
}

function addToast(id, title, body, type, hour, autohide, delay) {
  var toast = $([
    "<div id='toast-" + id + "' class='toast toast-" + type + "' data-autohide='" + autohide + "' data-delay='" + delay + "'>",
    "    <div class='toast-header'>",
    "      <strong class='mr-auto toast-title'>" + title + "</strong>",
    "      <small class='text-muted'>" + hour + "</small>",
    "      <button type='button' class='ml-2 mb-1 close' data-dismiss='toast'>&times;</button>",
    "    </div>",
    "    <div class='toast-body'>" + body + "</div>",
    "</div>"
  ].join("\n"));

  $("#toasts").append(toast);

  $('#toast-' + id).toast('show');
};

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

function SetEditor() {
  var editor;
  $('.editor').each(function (index) {
    var id = $(this).attr('id')

    editor = ace.edit(this);
    editor.setTheme("ace/theme/textmate");
    editor.session.setMode("ace/mode/ini");
  });
}