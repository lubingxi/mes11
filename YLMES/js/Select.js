
layui.define(['jquery', 'layer', 'form'], function (exports) {
    "use strict";
    var $ = layui.jquery, layer = layui.layer, form = layui.form
    var s = {};
    var a = null;
    var duf = null;
    var va = null;
    var obj = {
        render: function (o, fu) {
            if (a != null) {
                var w = new Worker("/js/workers.js")
                w.postMessage(o)
                w.onmessage = function (e) {
                    var html = e.data.html
                    var ob = e.data.ob
                    if (ob != false) {
                    $(ob).empty()
                    $(ob).append("<option value=''>请选择/搜索</option>")
                    $(ob).append(html)
                    }
                    if (duf != null) {
                        for (var i = 0; i < duf.length; i++) {
                            for (var da1 in va) {

                                $("tbody tr:eq(" + i + ")").find("." + da1).val(duf[i][va[da1]])
                            }
                        }
                    }
                    form.render()
                    if (!ob) {
                        w.terminate()
                    }
                    
                }
            }
            for (var ob in o) {
                if (o[ob].where == undefined) {
                    o[ob].where = {}
                }
                if (o[ob].data != null) {
                    if (typeof (o[ob].data) == "string") {
                        !function () {
                            $.ajax({
                                url: o[ob].data,
                                type: "get",
                                data: o[ob].where,
                                async: false,
                                dataType: "json",
                                success: function (data) {
                                    s[ob] = {}
                                    s[ob]["data"] = data
                                    $(ob).empty()
                                    $(ob).append("<option value=''>请选择/搜索</option>")
                                    for (var j = 0; j < data.length; j++) {
                                        if (o[ob].text != undefined && o[ob].text != null) {
                                            s[ob].text = o[ob].text
                                            s[ob].value = o[ob].value
                                            $(ob).append("<option value=" + data[j][o[ob].value] + ">" + data[j][o[ob].text] + "</option>")
                                        } else {
                                            $(ob).append("<option value=" + data[j].ID + ">" + data[j].Name + "</option>")
                                        }
                                    }
                                }
                            })
                        }()
                    }
                    if (o[ob].duf != undefined) {
                        $(ob).val(o[ob].duf)
                    }
                } else {
                    layer.msg("请输入地址或数据源", { icon: 5 })
                }
            }
            if (fu != undefined) {
                fu(s)
            }
        },
        reload: function (du, v, di) {
            a = di
            duf = du
            va = v
            obj.render(s)
        }
    }
    exports('Select', obj);
});