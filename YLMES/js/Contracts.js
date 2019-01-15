

layui.use(['layer', 'form', 'laydate', 'element', 'table'], function () {
    var form = layui.form, layer = layui.layer
    var laydate = layui.laydate;
    var element = layui.element;
    var layer = layui.layer;
    var table = layui.table;
    var tableins;
    $ = layui.jquery;
    var curDate = new Date();
    var getDate = new Date(curDate.setDate(curDate.getDate() + 7));
    var checkStatus = table.checkStatus('summary1')
    $("body div:first").css("height", "2350")
    laydate.render({
        elem: '#test1',
        value: new Date(),
        done: function (value, date) { }
    });
    laydate.render({
        elem: '#test1-1',
        value: new Date(),
        done: function (value, date) { }
    });
    laydate.render({
        elem: '#test1-2',
        value: new Date(),
        done: function (value, date) { }
    });
    laydate.render({
        elem: '#test1-3',
        value: getDate,
        done: function (value, date) { }
    });
    laydate.render({
        elem: '#test1-4',
        value: getDate,
        done: function (value, date) { }
    });
    function Transform(number) {
        if (Number(number) == 0) {
            return "零元整";
        }
        number = new String(parseFloat(number).toFixed(2));
        //分离整数与小数
        var num;
        var dig;
        if (number.indexOf(".") == -1) {
            num = number;
            dig = "";
        }
        else {
            num = number.substr(0, number.indexOf("."));
            dig = number.substr(number.indexOf(".") + 1, number.length);
        }
        //转换整数部分
        var i = 1;
        var len = num.length;
        var dw2 = new Array("", "万", "亿");//大单位
        var dw1 = new Array("拾", "佰", "千");//小单位
        var dw = new Array("", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖");//整数部分用
        var dws = new Array("零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖");//小数部分用
        var k1 = 0;//计小单位
        var k2 = 0;//计大单位
        var str = "";
        for (i = 1; i <= len; i++) {
            var n = num.charAt(len - i);
            if (n == "0" || n == 0) {
                if (k1 != 0)
                    str = str.substr(1, str.length - 1);

                if (str.substr(0, 1) != "零" && str.substr(0, 1) != "万" && str.substr(0, 1) != "亿") {
                    str = "零" + str;
                }
            }

            str = dw[Number(n)].concat(str);//加数字

            if (len - i - 1 >= 0)//在数字范围内
            {
                if (k1 != 3)//加小单位
                {
                    str = dw1[k1].concat(str);
                    k1++;
                }
                else//不加小单位，加大单位
                {
                    k1 = 0;
                    var temp = str.charAt(0);
                    if (temp == "万" || temp == "亿")//若大单位前没有数字则舍去大单位
                        str = str.substr(1, str.length - 1);
                    str = dw2[k2].concat(str);
                }
            }

            if (k1 == 3)//小单位到千则大单位进一
            {
                k2++;
            }
        }
        if (str.substr(str.length - 1, str.length) == "零") {
            str = str.substr(0, str.length - 1);
        }
        //转换小数部分
        var strdig = "";
        if (Number(dig) == 0) {
            strdig = "整";
        }
        else {
            for (i = 0; i < 2; i++) {
                var n = dig.charAt(i);
                if (i == 0 && Number(n) == 0) {
                    strdig = "零"
                }
                else {
                    strdig += dws[Number(n)] + (i == 0 ? "角" : "分");//加数字
                }
            }
        }
        str += "元" + strdig;
        return str;
    }
    $(document).ajaxStart(function () {
        layer.msg('加载中', {
            icon: 16
            , shade: 0.01
            , time: false
        });
    })
    function CountPrice(input) {
        if (input != null) {
            var sum = (Number($(".layui-table-total").find(".laytable-cell-1-0-7").text()) * $(input).val()) / 100
            $(input).next().text(":(" + Transform(sum) + ")")
        } else {
            $(".sp").find("span").each(function (i) {
                var sum = ($(".sp").find("span:eq(" + i + ")").prev().val() * Number($(".layui-table-total .laytable-cell-1-0-7").text())) / 100
                $(".sp").find("span:eq(" + i + ")").text(":(" + Transform(sum) + ")")
            })

        }
    }
    $(document).ajaxStop(function () {
        layer.closeAll();      
        $(".layui-elem-field input").each(function (i) {
            var len = $(".layui-elem-field input:eq(" + i + ")").val().length * 30;
            if (len == 0) {
                $(".layui-elem-field input:eq(" + i + ")").css("width", 10 + "px");
            }
            else {
                $(".layui-elem-field input:eq(" + i + ")").css("width", len+ "px");
            }
        })
        $(".layui-elem-field input").change(function () {
            var len = $(this).val().length * 30;
            if (len == 0) {
                $(this).css("width", 10 + "px");
            }
            else {
                $(this).css("width", len+ "px");
            }
        });
        $(".layui-elem-field input").keyup(function () {
            var len = $(this).val().length * 30;

            if (len == 0) {
                $(this).css("width", 10 + "px");
            }
            else {
                $(this).css("width", len + "px");
            }
        });

        $(".layui-elem-field input").css("padding", "0px");
        $(".print").click(function () {
            $(".layui-form-radio").each(function (i) {
                if ($(".layui-form-radio:eq(" + i + ")").attr("class") != "layui-unselect layui-form-radio layui-form-radioed") {
                    $(this).parents(".aa").hide();
                }
            });
            $(":input").css("border", "0px");
            $("button").hide();
            $("i").hide();

            $(":input").each(function (i) {
                $(":input:eq(" + i + ")").attr("value", $(":input:eq(" + i + ")").val());
            })
            bdhtml = window.document.body.innerHTML;
            sprnstr = "<!--startprint-->";
            eprnstr = "<!--endprint-->";
            prnhtml = bdhtml.substring(bdhtml.indexOf(sprnstr) + 17);
            prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
            window.document.body.innerHTML = prnhtml;
            window.print();
        })
    });
    //form.on('radio', function (data) {
    //    alert($(data.elem).next().attr("class"))
    //});  
    $.get("/ApplierList/checkApplierName", function (data) {
        for (var i = 0; i < data.length; i++) {
            if (data != null && data != "") {

                $("[name=Party_b]").append("<option value='" + data[i].Name + "'>" + data[i].Name + "</option>")
            }

        }
        form.render();
    })

    tableins = table.render({
        elem: '#summary1'
        , url: "/PurchasingManage/checkSummary?ids=" + $(".appvalue").val()

        , cols: [[
            { hide: true }
            , { type: "numbers", width: 50, style: 'text-align:center;vertical-align:middle;', hide: true }
            , { field: 'MaterialID', hide: true }
            , { field: 'PartNumber', width: 240, title: '名称', style: 'text-align:center;vertical-align:middle;', totalRowText: '合计：' }
            , { field: 'PartSpec', width: 220, title: '规格', style: 'text-align:center;vertical-align:middle;' }
            , { field: 'ActPurchaseQTY', width: 140, title: '数量', style: 'text-align:center;vertical-align:middle;', totalRow: true }
            , { title: '单位', width: 140 }
            , { field: 'UnitPrice', width: 250, title: '含税单价（元）', edit: "text", style: 'text-align:center;vertical-align:middle;', totalRow: true }
            , { field: 'TotalPrice', width: 250, title: '含税金额（元）', style: 'text-align:center;vertical-align:middle;', totalRow: true }
            , { field: 'beiz', width: 210, title: '备注', style: 'text-align:center;vertical-align:middle;' }
        ]]
        , done: function (res) {

            for (var i = 0; i < res.data.length; i++) {
                res.data[i]["LAY_CHECKED"] = "true";
            }
            checkStatus = table.checkStatus('summary1')
            $(".sp .layui-input").keyup(function () {
                if (isNaN($(this).val())) {
                    layer.msg("请输入数字")
                } else {
                    CountPrice(this)
                }
            })
        }
    })
    $("body").append("<div><button class='layui-btn layui-btn-radius layui-btn-primary print'>打印</button><button class='layui-btn layui-btn-radius layui-btn-primary OK'>确认</button></div>")
    $("body").prepend("<!--startprint-->")
    $(".print").parent().css({ "position": "fixed", "bottom": "130px", "right": "115px" })
    $(".print").after("<!--endprint-->")
    $(".OK").click(function () {
        var Sum = 0;
        for (var i = 0; i < checkStatus.data.length; i++) {
            Sum = checkStatus.data[i].TotalPrice + Sum
        }
        var indexStut = 0;
        for (var i = 0; i < checkStatus.data.length; i++) {
            $.ajax({
                url: "/PurchasingManage/OkStut",
                type: "post",
                async: false,
                data: { ids: checkStatus.data[i].MaterialID, Price: checkStatus.data[i].UnitPrice, Money: checkStatus.data[i].TotalPrice, Appid: $(".appid").val(), indexStut: indexStut, Sum: Sum },
                dataType: "text",
                success: function (data) {
                    indexStut = 1;
                }
            })
        }
        layer.msg("已确认")

    })
    table.on('edit(summary1)', function (obj) {
        var Price = obj.data.UnitPrice
        var Count = obj.data.ActPurchaseQTY
        obj.data.TotalPrice = Number(Price * Count).toFixed()
        table.cache.summary1[$("tbody tr").index(obj.tr)].TotalPrice = obj.data.TotalPrice
        tableins.reload({
            url: ""
            , data: table.cache.summary1
        })
        CountPrice(null)
    });
    form.on('select', function (data) {
        var value = data.value;
        if (value == 0) {
            $(".se").val(0)
            form.render()
        } else if (value == 1) {
            $(".se").val(1)
            form.render()
        }
    })
    form.on('select(Party_b)', function (data) {
        value = data.value;

        $.get("/PurchasingManage/ByNamefindApplier", { ApplierName: value }, function (data) {
            $("[name=Tel]").val(data[0].Tel)
            $("[name=Party_b]").val(data[0].Name)
            $("[name=Fax]").val(data[0].Fax)
            $("[name=Bank]").val(data[0].Bank)
            $("[name=Principal]").val(data[0].Principal)
            $("[name=Representative]").val(data[0].Representative)
            $("[name=Mobile]").val(data[0].Mobile)
            $("[name=Contact]").val(data[0].Contact)
            $("[name=Address]").val(data[0].Address)
            $("[name=Account]").val(data[0].Account)

        })
    })

    $("i").click(function () {
        layer.open({
            type: 2,
            content: '/ApplierList/ApplierAdd',

            maxmin: true,
            success: function (layero, index) {
                var body = layer.getChildFrame('body', index);
                body.find(".id").val(1);
                layer.full(index)
                form.render();
            },
            end: function () {
                if ($(".idapp").val() == 1) {
                    $(".idapp").attr("value", 0)
                    $.get("/PurchasingManage/checkApplierId", function (id) {
                        $.ajax({
                            url: "/PurchasingManage/ByIdfindApplier",
                            type: "get",
                            data: { id: id },
                            dataType: "json",
                            success: function (data) {
                                if (data != null) {
                                    $("[name=Party_b]").append("<option value='" + data[0].Name + "'>" + data[0].Name + "</option>")
                                    $("[name=Party_b]").val(data[0].Name)
                                    $("[name=Tel]").val(data[0].Tel)
                                    $("[name=Fax]").val(data[0].Fax)
                                    $("[name=Bank]").val(data[0].Bank)
                                    $("[name=Principal]").val(data[0].Principal)
                                    $("[name=Representative]").val(data[0].Representative)
                                    $("[name=Mobile]").val(data[0].Mobile)
                                    $("[name=Contact]").val(data[0].Contact)
                                    $("[name=Address]").val(data[0].Address)
                                    $("[name=Account]").val(data[0].Account)
                                }
                                form.render()
                            }
                        })
                    })
                }
            }
        });
    })
    if ($(".appid").val() != "") {
        var appid = $(".appid").val()
        var appvalue = new Array($(".appvalue").val())
        $.post("/PurchasingManage/UpDefaultStu", { mid: appvalue, appid: appid, cuid: $(".cuid").val() })
        $.ajax({
            url: "/PurchasingManage/ByIdfindApplier",
            type: "get",
            data: { id: appid },
            dataType: "json",
            success: function (data) {
                if (data != null) {

                    $("[name=Party_b]").val(data[0].Name)
                    $("[name=Tel]").val(data[0].Tel)
                    $("[name=Fax]").val(data[0].Fax)
                    $("[name=Bank]").val(data[0].Bank)
                    $("[name=Principal]").val(data[0].Principal)
                    $("[name=Representative]").val(data[0].Representative)
                    $("[name=Mobile]").val(data[0].Mobile)
                    $("[name=Contact]").val(data[0].Contact)
                    $("[name=Address]").val(data[0].Address)
                    $("[name=Account]").val(data[0].Account)
                }
                form.render()
            }
        })

    }
});