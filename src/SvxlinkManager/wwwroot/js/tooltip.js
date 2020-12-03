function SetToolTips() {
  $('[data-toggle="tooltip"]').tooltip({
    html: true
  })
}

function SetPopOver() {
  $('[data-toggle="popover"]').popover()
}

function ShowError() {
  $("#error-alert").alert();
}