﻿function SetEditor() {
  $('.editor').each(function (index) {
    var id = $(this).attr('id');
    var input: HTMLInputElement = <HTMLInputElement>document.getElementById(id + '-textarea');
    $('#' + id + '-textarea').hide();

    var editor: AceAjax.Editor = ace.edit(this);
    editor.setTheme("ace/theme/textmate");
    editor.session.setMode("ace/mode/ini");

    editor.getSession().on('change', function () {
      input.value = editor.getSession().getValue();
      var event = new Event('change');
      input.dispatchEvent(event);
    })
  });
}