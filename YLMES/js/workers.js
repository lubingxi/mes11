
onmessage = function (e) {
    var o = e.data
    for (var ob in o) {
        var html = "";
        if (typeof (o[ob].data) == "string") {
            ob = false;
        }
        if (ob != false) {
            for (var j = 0; j < o[ob].data.length; j++) {
                if (o[ob].text != undefined && o[ob].text != null) {
                    html += "<option value=" + o[ob].data[j][o[ob].value] + ">" + o[ob].data[j][o[ob].text] + "</option>"
                } else {
                    html += "<option value=" + o[ob].data[j].ID + ">" + o[ob].data[j].Name + "</option>"
                }
            }
            postMessage({ html: html, ob: ob });
        }
    }
    postMessage({ html: false, ob: false });
}