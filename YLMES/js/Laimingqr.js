
layui.config({
    base: '/js/'
}
).use(['laydate', 'table', 'layer', 'form', 'Select'], function () {
    "use strict"
    var table = layui.table, layer = layui.layer, form = layui.form, laydate = layui.laydate, $ = layui.jquery;
    var dates = $("#dates").val();
    var datee = $("#datee").val();
    var select = layui.Select
    var table2;

    var val
    var val1
    var key = function () {
        if (event.code == "Equal") { // 按 =号平衡   
            try {
                val = Number($(".layui-table-total [data-field=name2] div").text())
                val1 = Number($(".layui-table-total [data-field=name3] div").text())
                var v;
                var ind = $("tbody tr").index($(this).parents("tr"))
                var na = $(this).attr("data-field")
                if (na == 'name2') {
                    table.cache.myBtn[ind].name3 = Number(0).toFixed(2)
                    v = (val1 - val).toFixed(2)
                } else if (na == 'name3') {
                    table.cache.myBtn[ind].name2 = Number(0).toFixed(2)
                    v = (val - val1).toFixed(2)
                }
                table.cache.myBtn[ind][na] = v
                setTimeout(() => {
                    $(this).find("input").val("")
                    $(this).find("input").val(v)
                    $(this).find("input").blur()
                })
            } catch (a) {


            }
        }
    }
    var key2 = function () {
        var boo = false;
        var e;
        var trIndex;
        $(this).parents("tr").prev().find(".layui-table-cell").each(function () {

            $(this).text() == "" || 0 ?
                boo = true
                :
                boo = false

            if (boo) {
                e = $(this).parents("td").attr("data-key")
                trIndex = $(this).parents(".layui-table-body").find("tr").index($(this).parents("tr"))
                return false;
            }
        })
        if (boo) {
            layer.msg("请填写完整上一行")
            $(".layui-table-body tr:eq(" + trIndex + ")").find("[data-key=" + e + "]").click()
        }
    }
    var even = function () {
        if (event.code == "Enter") {
            var tdIndex = $(this).parents("tr").children().index(this)
            var trIndex = $(this).parents(".layui-table-body").find("tr").index($(this).parents("tr"))
            if ($(this).next().find("select").length > 0) {
                $(this).next().find(".layui-input").focus()
                $(this).next().find(".layui-input").click()
            } else {
                setTimeout(() => {
                    var a = $("[lay-id=myBtn] .layui-table-body tr:eq(" + trIndex + ")").find("td:eq(" + tdIndex + ")")
                    a.next().click()
                    if (a.next().find("input").length > 0) {
                        a.next().find("input").select()
                    } else {
                        if (a.parents("tr").next().length > 0) {
                            a.parents("tr").next().find("td")[1].click()
                        } else {
                            a.find("input").blur()
                            $('#xzy2').click()
                            var length = $("[lay-id=myBtn] .layui-table-body tr").length - 1
                            $("[lay-id=myBtn] .layui-table-body tr:eq(" + length + ")").find("td:eq(1)").click()
                        }
                    }
                })
            }
        }
    }
    var inputChange = function () {
        $(this).parent().next().find("[class=layui-this]").attr("class", "")
        var arr = []
        var arr1 = []
        setTimeout(() => {
            var aa = $(this).val()
            var reg = new RegExp(aa)
            var boo = true
            $(this).parent().next().find("dt").each(function () {
                if (!reg.test(this.innerHTML)) {
                    $(this).attr("class", "layui-hide")
                } else {
                    $(this).attr("class", "")
                    arr.push($(this).attr("name"))
                }
            })
            $(this).parent().next().find("dd").each(function () {
                arr.forEach((va) => {
                    if (va==$(this).attr("name")) {
                        $(this).attr("class", "")
                        $(".layui-select-none").remove()
                    }
                })
                if (!$(this).attr("class").includes("layui-hide")) {
                    if (!arr1.includes($(this).attr("name"))) {
                        arr1.push($(this).attr("name"))
                    }
                    if (boo) {
                        if (this.innerHTML != "无匹配项") {
                            $(this).attr("class", "layui-this")
                            boo = false
                        }
                    }
                }
            })
            $(this).parent().next().find("dt").each(function () {
                arr1.forEach(va => {
                    if (va == $(this).attr("name")) {
                        $(this).attr("class", "")
                    }
                })
            })
        }, 150)
    }
    $(document).on("keydown", "tbody td", even)
    $(document).on("keydown", 'td[data-field="name2"],td[data-field="name3"],td[data-field="name"] ', key2);//键盘事件
    $(document).on("keydown", 'tbody td[data-field="name2"],td[data-field="name3"]', key);//键盘事件
    $(document).on("input propertychange", ".layui-form-select .layui-input", inputChange)
    var today2 = new Date();
    var s3 = today2.getFullYear() + "-" + (today2.getMonth() + 1) + "-" + today2.getDate();

    table2 = table.render({
        elem: '#tw'
        , url: '/Financial/SeAccuntingListpz'
        , page: true
        , limit: 15

        , cols: [[
            { field: '', hide: true }
            , { field: '编号', width: 60, fixed: 'left', title: '序号' }
            , { field: '凭证单号', title: '凭证单号' }
            , { field: '制单人', title: '制单人' }
            , { field: '记字号', title: '记字号' }
            , { field: '时间', title: '凭证日期' }
            , { field: 'right', align: 'center', title: '操作', toolbar: '#barDemo2' }
        ]]
    });
    //新增    
    var s2;
    //保存
    $('#b2c').on('click', function () {
        var bo = false
        val = Number($(".layui-table-total [data-field=name2] div").text())
        val1 = Number($(".layui-table-total [data-field=name3] div").text())
        table.cache.myBtn.forEach(function (s) {
            if (s.name == "") {
                bo = true
            } else
                if (s.name1 == "" || s.name1 == undefined) {
                    bo = true
                } else
                    if (s.name4 == "" || s.name4 == undefined) {
                        bo = true
                    } else if (s.name2 != "0.00" || s.name3 != "0.00") {
                        bo = false
                    }
        })
        if (val == val1) {
            if (!bo) {
                $.post("/Financial/AccuntingAD",
                    {
                        data: table.cache.myBtn.map(x => {
                            return {
                                name: x.name, name1: x.name1, name2: x.name2, name3: x.name3, name4: x.name4, numBering: x.numBering
                            }
                        }),
                        pzdanh: s2, jzh: $('#jzh2').val()
                    })
                layer.closeAll()
                layer.msg("保存成功")
                table2.reload({
                    url: '/Financial/SeAccuntingListpz'
                })
            } else {
                layer.msg("请填写完整")
            }
        } else {
            layer.msg("借贷不平衡")
        }

    });
    //新增 空行
    $('#xzy2').on('click', function () {
        var ff = true;
        $(".gss8").each(function () {
            if ($(this).val() == '') {
                ff = false;
                return;
            }
        });
        if (ff) {
            $(".gssb").each(function () {
                if ($(this).val() == '') {
                    ff = false;
                    return;
                }
            });
        }
        if (ff) {
            $(".gss6").each(function () {
                if ($(this).val() == '') {
                    ff = false;
                    return;
                }
            });
        }
        var data2 = table.cache.myBtn;
        data2.forEach(function (n) {
            if (n.name == '' || (n.name2 == '0.00' && n.name3 == '0.00')) {
                ff = false;
                return;
            }
        })
        if (!ff) {
            layer.msg('请填写完整凭证！');

        } else {
            var tr = document.querySelector("[lay-id=myBtn] .layui-table-body tbody").lastChild
            tr = tr.cloneNode(true)
            tr.setAttribute("data-index", table.cache.myBtn.length)
            document.querySelector("[lay-id=myBtn] .layui-table-body tbody").appendChild(tr)
            var selectFilter = "select" + table.cache.myBtn.length
            $(tr).attr("lay-filter", selectFilter)
            $(tr).addClass("layui-form")
            $(tr).find(".gss8").val(table.cache.myBtn[table.cache.myBtn.length - 1].name1)
            $(tr).find(".gssb").val(table.cache.myBtn[table.cache.myBtn.length - 1].name4)
            $(tr).find(".gss6").val(table.cache.myBtn[table.cache.myBtn.length - 1].numBering)
            $(tr).find(".laytable-cell-numbers").text(table.cache.myBtn.length + 1)
            $(tr).find("[data-field=name2] div").text(Number(0).toFixed(2))
            $(tr).find("[data-field=name3] div").text(Number(0).toFixed(2))
            form.render(null, selectFilter)
            table.cache.myBtn.push(
                {
                    name: table.cache.myBtn[table.cache.myBtn.length - 1].name,
                    name2: "0.00",
                    name1: table.cache.myBtn[table.cache.myBtn.length - 1].name1,
                    name4: table.cache.myBtn[table.cache.myBtn.length - 1].name4,
                    numBering: table.cache.myBtn[table.cache.myBtn.length - 1].numBering,
                    name3: "0.00"
                })
        }
    })
    //新增 
    $('#adds').on('click', function () {
        layer.open({
            type: 1
            , title: '新增凭证'
            , area: ['100%', '100%']
            , content: $("#for7")
            , skin: ['layui-layer-molv']
            , success: function () {
                table.render({
                    elem: '#myBtn'
                    , limit: 15
                    , height: 'full-300'
                    , totalRow: true
                    , cols: [[
                        { type: "numbers", title: '序号', totalRowText: '合计' }
                        , { field: 'name', title: '摘要', edit: 'text' }
                        , {
                            field: 'name1', templet: function (d) {
                                
                                var a = JSON.parse(sessionStorage.select)
                                var select = a["gss8"].data.replace("value=" + d.name1, "value=" + d.name1 + " selected")
                                return select
                            },
                            title: '科目名称'
                        }
                        , {
                            field: 'name4', width: 160, templet: function (d) {
                                var a = JSON.parse(sessionStorage.select)

                                var select = a["gssb"].data.replace("value=" + d.name4, "value=" + d.name4 + " selected")
                                return select
                            },
                            title: '部门'
                        }
                        , {
                            field: 'numBering', templet: function (d) {
                                var a = JSON.parse(sessionStorage.select)

                                var select = a["gss6"].data.replace("value=" + d.numBering, "value=" + d.numBering + " selected")
                                return select
                            },
                            title: '项目编号'
                        }
                        , { field: 'name2', title: '借方', width: 100, edit: 'text', totalRow: true }
                        , { field: 'name3', edit: 'text', width: 100, title: '贷方', totalRow: true }
                        , { field: 'right', width: 160, align: 'center', title: '操作', toolbar: '#barDemo3' }

                    ]],
                    data: [{ name: "", name2: "0.00", name1: "0", name4: "0", numBering: "0", name3: "0.00" },
                    { name: "", name2: "0.00", name1: "0", name4: "0", numBering: "0", name3: "0.00" }]
                })
            }
        });
        //记字号初始赋值
        laydate.render({
            elem: '#jrq'
            , value: s3
            , isInitValue: true
            , theme: 'molv'
            
        });
        $.ajax({
            async: false,//ajax先执行，默认为ture
            url: "/Financial/SeAccuntingJZH",
            data: {
                date: s3
            },
            type: "post",
            dataType: "text",
            success: function (data) {
                if (data != null) {
                    var dataObj = eval("(" + data + ")");//转换为json对象 
                    $.each(dataObj, function (name, value) {
                        $('#jzh2').val(dataObj[name].记字号);
                        $('#lb8').text('记字号：' + dataObj[name].记字号);
                    });
                }
                form.render();
            }
        });
        var today = new Date();
        s2 = 'JD' + today.getFullYear() + "" + today.getMonth() + "" + today.getDate() + "" + today.getHours() + "" + today.getMinutes() + "" + today.getSeconds();
        $('#lb5').text('记账单号：' + s2);
    })
    //单元格监听
    table.on('edit(myBtn)', function (obj) {
        var t = obj.value
        var ind = $("tbody tr").index(obj.tr)
        if (obj.field == 'name2' || obj.field == 'name3') {
            if (isNaN(obj.value) || obj.value == "") {
                layer.msg("只可以输入数字", { icon: 5 })
                t = 0;
            }
            if (obj.field == 'name2') {
                table.cache.myBtn[ind]['name3'] = Number(0).toFixed(2)
                $(obj.tr).find("[data-field=name3] div").text(Number(0).toFixed(2))
            } else if (obj.field == 'name3') {
                $(obj.tr).find("[data-field=name2] div").text(Number(0).toFixed(2))
                table.cache.myBtn[ind]['name2'] = Number(0).toFixed(2)
            }
            setTimeout(() => {
                $(obj.tr).find("[data-field=" + obj.field + "] div").text(Number(t).toFixed(2))
            })
            table.cache.myBtn[ind][obj.field] = Number(t).toFixed(2)
            var num1 = 0
            var num2 = 0
            table.cache.myBtn.forEach(function (x) {
                num1 += Number(x.name2)
                num2 += Number(x.name3)
            })
            $(".layui-table-total [data-field=name2] div").text(num1.toFixed(2))
            $(".layui-table-total [data-field=name3] div").text(num2.toFixed(2))
        }
    });
    form.on('select', function (obj) {
        var ind = $("tbody tr").index($(obj.elem).parents("tr"))

        var n = $(obj.elem).parents("td").attr("data-field")

        table.cache.myBtn[ind][n] = obj.value
        var value = obj.value
        if ($(obj.elem).parents("td").next().find("select").length > 0) {
            var a = Array.from($(obj.elem).parents("td").next().find("select").children(), function (x) {
                return { value: x.getAttribute("value"), name: x.innerHTML }
            })
            $(obj.elem).parents("td").find("dt").each(function () {
                if (new RegExp(String(obj.value).substring(0, 4)).test($(this).text())) {
                    value = $(this).text()
                }
            })
            if (isNaN(value)) {
                var b = a.find(x => value.includes(...new String(x.name)))

                if (b != undefined) {
                    $(obj.elem).parents("td").next().find("select").val(b.value)
                }
            }
        }
    })
    //附件上传取消
    laydate.render({
        elem: '#datee2'
        , theme: 'molv'
    });
    laydate.render({
        elem: '#dates2'
        , theme: 'molv'
    });
    //模糊查询
    $("#Search2").on('click', function () {
        var dates = $("#dates2").val().trim();
        var datee = $("#datee2").val().trim();
        table2.reload({
            where: {
                dates: dates
                , datee: datee
            }
            , page: {
                curr: 1
            }
        });
        form.render();
    });
    table.on('tool(myBtn)', function (obj) {
        var layEvent = obj.event; //获得 lay-event 对应的值
        if (layEvent === 'del') {
            obj.del();
            table.cache.myBtn = table.cache.myBtn.filter(x => x.length != 0)
            $(obj.tr).remove()
        }
    })
    table.on('tool(demo)', function (obj) { //注：tool 是工具条事件名，test 是 table 原始容器的属性 lay-filter="对应的值"
        var data = obj.data //获得当前行数据
            , layEvent = obj.event, danh2 = data.凭证单号;
        if (layEvent === 'check') {
            layer.open({
                type: 1
                , title: '记账凭证'
                , area: ['80%', '80%']
                , content: $("#for9")
                , skin: ['layui-layer-molv']
            });
            $('#lb').text('单位名称：友力智能');
            $('#lb2').text('日期：' + data.时间);
            $('#lb3').text('记字号：' + data.记字号);
            $('#lb4').text('制单人：' + data.制单人);
            table.render({
                elem: '#tw1'
                , url: '/Financial/SeAccuntingse?danh=' + danh2
                , totalRow: true
                , limit: 15
                , cols: [[
                    { field: '', hide: true }
                    , {
                        type: "numbers", title: '序号'
                    }
                    //, { field: '序号', width: 60, fixed: 'left', title: '序号' }
                    , { field: '摘要', title: '摘要', totalRowText: '合计' }
                    , { field: '科目名称', title: '科目名称' }
                    , { field: 'Dept', width: 160, title: '部门' }
                    , { field: 'Cnumber', title: '项目编号' }
                    , { field: '借方', width: 120, align: 'center', title: '借方', totalRow: true }
                    , { field: '贷方', width: 120, align: 'center', title: '贷方', totalRow: true }

                ]]
            });
        } else if (layEvent === 'delpz') {
            layer.confirm('真的删除么？', function (index) {
                $.ajax({
                    url: "/Financial/DelAccounting",
                    data: { id: data.编号, danh: data.凭证单号 },
                    type: "post",
                    dataType: "text",
                    success: function (data) {
                        if (data == "True") {
                            layer.msg('删除成功！');
                        } else {
                            layer.msg('删除失败！');
                        }
                        table2.reload({
                            where: {
                                dates: dates
                                , datee: datee
                            }
                            , page: {
                                curr: 1
                            }
                        });
                        form.render();
                    }
                });
            });
        }
    });
    select.render({
        'gss8': { data: "/Financial/SeCaptionall", value: "name", text: "name" },
        'gssb': { data: "/Financial/SeDepts", value: "编号", text: "部门" },
        'gss6': { data: "/Financial/SeContract", value: "id", text: "name" }
    }, function (data) {
        var obj = {};
            var html = '<select lay-search><option value="">请选择</option>';
            var htmls = "<optgroup name='0000' label='其他分类'>";
        data["gss8"].data.forEach(x => {
            if (obj[x.name.substring(0, 4)]) {
                obj[x.name.substring(0, 4)].push(x.name)
            } else {
                obj[x.name.substring(0, 4)] = []
                obj[x.name.substring(0, 4)].push(x.name)
            }
        })
            for (var a in obj) {
                if (obj[a].length > 1) {
                    html += "<optgroup name='" + String(obj[a][0].substring(0,4)) + "' label=" + obj[a][0] + ">";
                obj[a].forEach((x,i) => {
                    if (i != 0) {
                        html += "<option name='" + String(x.substring(0,4)) + "' value=" + x + ">" + x + "</option>";
                    }
                })
                html += "</optgroup>";
            } else {
                    htmls += "<option name='0000' value=" + obj[a].toString() + " >" + obj[a].toString() + "</option>";
            }
        }
        htmls += "</optgroup></select>";
            data["gss8"].data = html + htmls;
            html = "<select lay-search><option value=''>请选择</option>";
     
            data["gssb"].data.forEach(x => {
                html += "<option value=" + x["编号"] + ">" + x["部门"] + "</option>";
            })
            data["gssb"].data = html += "</select>";
            html = "<select lay-search><option value=''>请选择</option>";
            data["gss6"].data.map(x => {
                html += "<option value=" + x.id + ">" + x.name + "</option>";
            })
            data["gss6"].data = html += "</select>";
        sessionStorage.select = JSON.stringify(data)
    });
})