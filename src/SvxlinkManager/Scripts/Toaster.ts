﻿function addToast(id, title, body, type, hour, autohide, delay) {
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
}