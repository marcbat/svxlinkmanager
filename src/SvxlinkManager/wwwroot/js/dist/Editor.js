function SetEditor() {
    var editor;
    $('.editor').each(function (index) {
        editor = ace.edit(this);
        editor.setTheme("ace/theme/textmate");
        editor.session.setMode("ace/mode/ini");
    });
}
//# sourceMappingURL=Editor.js.map