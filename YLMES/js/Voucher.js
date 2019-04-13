$(function () {
    layui.config({
        base: '/js/'
    }
    ).use(['upload', 'laydate', 'table', 'layer', 'form', 'Select'], function () {
        var table = layui.table, layer = layui.layer, form = layui.form, laydate = layui.laydate, upload = layui.upload;
        var dates = $("#dates").val();
        var datee = $("#datee").val();
        var num = 0;
        var select = layui.Select
        var note5='',km='',ytu = '', mans = '', manf = '', manb = '', manl2 = '', money = '', note = '', date4 = '', dept2 = '';
        var index2 = Object;
        var index5 = Object;
        var index3 = Object, index4 = Object;
        var name2 = '', status = '', id = '';
        var flag = 0;
        var table2 = Object;
        var table3 = Object;
        table2 = table.render({
            elem: '#tw'
            , url: '/Financial/SeLaimingListpz'
            , page: true
            , limit: 15
            , done: function () {
                $("tr a").hide();
                $("tr").hover(function () {
                    $(this).children().last().children().children().toggle();
                });
            }
            , cols: [[
                { field: '', hide: true }
                , { field: '编号', width: 60, fixed: 'left', title: '序号' }
                , { field: '凭证单号', width: 150, title: '凭证单号' }
                , { field: '部门', width: 150, title: '部门', style: "color: #009688" }
                , { field: '项目编号', width: 150, title: '项目编号' }
                , { field: '制单人', width: 150, title: '制单人' }
                , { field: '记字号', width: 150, title: '记字号' }
                , { field: '凭证日期', width: 150, title: '凭证日期' }
                , { field: '状态', width: 150, title: '状态' }         
                , { field: 'right', width: 280, align: 'center', title: '操作', toolbar: '#barDemo' }
            ]]
        });
        laydate.render({
            elem: '#date4'
            , theme: 'molv'
        });
        laydate.render({
            elem: '#date42'
            , theme: 'molv'
        });
        $('#demo3').on('click', '.imgt', function () {
            $(".imgg").attr("src", $(this).attr("src"))
            layer.open({
                type: 1,
                title: false,
                closeBtn: 0,
                area: '516px',
                skin: 'layui-layer-nobg', //没有背景色
                shadeClose: true,
                content: $(".imgg")
            });
        })
        //模糊查询
        $("#Search").click(function () {
            var dates = $("#dates").val().trim();
            var datee = $("#datee").val().trim();
            table2.reload({
                where: {
                    dates: dates
                    , datee: datee,
                    status: $('#gssz').val()
                }
                , page: {
                    curr: 1
                }
            });
            form.render();
        });
        //删除   
        var tt;
        $('#csy').on('click', function () {
            alert(1);
            $("#myBtn").find("tr:last").remove();
            alert(2);
            form.render();
           
        });
        //新增    
        var tt;
        var p = 0;
   
        $('#xzy').on('click', function () {
            //console.log(table)
            //var data2 = table.cache.myBtn;
            //data2.forEach(function (n, i) {

            //    //alert(n.name1);

            //})
          
           table.cache.myBtn.push({
                name: "",
                name2: "",
                name3: "",
                name1: ""
            })
            tt.reload({
                data: table.cache.myBtn
            })
            //table.resize('myBtn');
            select.render({
                'tr .gssz2': { data: "/Financial/SeCaptionall", value: "name", text: "name" }
            });
            //$('.gssz2').append("<option value='" + 2 + "' selected >" + (++p) + "</option>")
            form.render();
        });

        //新增 xzy
        $('#add2').on('click', function () {
            var checkStatus = table.checkStatus("tw9")
                , data = checkStatus.data;
            var delList = [];
            var cate = new Array();
    
            layer.open({
                type: 1
                , title: '单号查看'
                , area: ['100%', '100%']
                , content: $("#for7")
                , skin: ['layui-layer-molv'],
                success: function () {
                    tt=  table.render({
                        elem: '#myBtn'
                        , limit: 15
                        , totalRow: true
                        , cols: [[
                            {
                               type: "numbers", title: '序号', totalRowText: '合计'}
                            , { field: 'name', title: '摘要', width: 120,edit: 'text'}
                            , { field: 'name1', title: '科目名称', templet: '#Category' }
                            , { field: 'name2', title: '借方', edit: 'text', totalRow: true }
                            , { field: 'name3', edit: 'text', title: '贷方', totalRow: true  }
                        ]],
                        data: []
                    });
                }
                //, done: function (res) {
                //    for (var i = 0; i < res.data.length; i++) {
                //        cate[i] = res.data[i].name1;s
                //    }
                //    $.get("/ApplierList/checkPurchase", function (data) {
                //        if (data != null) {
                //            $(".CategoryId").empty();
                //            for (var j = 0; j < MaterialType.length; j++) {
                //                for (var i = 0; i < data.length; i++) {
                //                    if (MaterialType[j] == data[i].ID) {
                //                        $(".CategoryId:eq(" + j + ")").append("<option value='" + data[i].ID + "' selected >" + data[i].Name + "</option>")
                //                    } else {
                //                        $(".CategoryId:eq(" + j + ")").append("<option value='" + data[i].ID + "'>" + data[i].Name + "</option>")
                //                    }
                //                }
                //            }
                //        }
                //        form.render(null, 'LAY-table-1');
                //    })
                //}
            });
            var t = 0;
            data.forEach(function (n, i) {
             
                //delList.push(n.编号);
                table.cache.myBtn.push({
                    name: n.用途,
                    //name1: n.账号,
                    name2: n.金额,
                    name3: (++t) 
                })

                tt.reload({
                    data: table.cache.myBtn
                })
                table.resize('myBtn');
              
                form.render();
            })
            //$('.gssz2').append("<option value='" + 0 + "' selected >" + (++t) + "</option>")
            select.render({
                '.gssz2': { data: "/Financial/SeCaptionall", value: "name", text: "name" }
            });
            form.render();
         })
        //新增
        $('#add').on('click', function () {

            flag = 1;
            select.render({
                '#gss': { data: "/Financial/SeDepts", value: "ID", text: "Dept" }
            }, function (data) {
                layui.each(data["#gss"].data, function (index, value) {
                    objData[value.ID] = value;
                })
            });
            layer.open({
                type: 1
                , title: '单号查看'
                , area: ['100%', '100%']
                , content: $("#for6")
                , skin: ['layui-layer-molv']
            });
            table3 = table.render({
                elem: '#tw9'
                , url: '/Financial/SeLaimingListzfpz'
               
                , page: true
                , limit: 15
                , cols: [[
                    { type: 'checkbox', fixed: 'left' }
                    , { field: '编号', width: 60, fixed: 'left', title: '序号' }
                    , { field: '费用部门', width: 150, title: '费用部门' }
                    , { field: '单号', width: 170, title: '单号(查看)', event: 'xi', templet: '#cc' }
                    , { field: '用途', width: 150, title: '用途', style: "color: #009688" }
                    , { field: '金额', width: 150, title: '金额' }
                    , { field: '支付时间2', width: 150, title: '支付时间2' }
                    , { field: '附件数量', width: 150, title: '附件数量'}
                    , { field: '领用人', width: 150, title: '领用人' }
                    , { field: '报销人', width: 150, title: '报销人' }
                    , { field: '报销部门主管复核', width: 150, title: '报销部门主管复核' }
                    , { field: '账号', width: 150, title: '支付账号' }
                    //, { field: '状态', width: 150, title: '状态' }
                    , { field: '备注', width: 150, title: '备注' }
                    , { field: '录入状态', width: 150, title: '录入状态' }
                    //, { field: 'zt', width: 150, title: '状态', event: 'xi', sort: true, templet: '#cca' }
                    , { field: 'right', width: 185, align: 'center', title: '操作', toolbar: '#barDemo' }
                ]]
            });
  
        });
        //新增取消
        $("#qx").on('click', function () {
            layer.close(index2);
        });
        //修改取消
        $("#q2x").on('click', function () {
            layer.close(index3);
        });
        //记账
        $("#bc5").on('click', function () {
            km = $('#gss5').val();
            note5 = $('textarea[name= note5]').val();
            if (km.trim() != '') {
                $.ajax({
                    url: "/Financial/UpLaimingpz",
                    data: {
                        name: $('#Sessioname').val()
                        , km: km
                        , note5: note5
                        , id: id
                    },
                    type: "post",
                    dataType: "text",
                    success: function (data) {
                        if (data == "True") {
                            layer.msg('记账成功！');
                            table2.reload({
                                where: {
                                    dates: datee
                                    , datee: dates,
                                    status: $('#gssz').val()
                                   
                                }
                                , page: {
                                    curr: 1
                                }
                            });
                        }
                        if (data == "False") {
                            layer.msg('记账失败！');
                        }
                        layer.close(index5);
                        form.render();
                    }
                });
            }
      
        });
        //记账取消
        $("#qx5").on('click', function () {
            layer.close(index5);
        });
        //自定义验证规则--新增科目类别form表单验证
        form.verify({
            gss5: function (value) {
                if (value.toString().trim() == "") {
                    return '必填不能为空！';
                }   
            }
           , gss: function (value) {
                if (value.toString().trim() == "") {
                    return '必填不能为空！';
                }
                dept2 = $('#gss').val();
            }
            , gss2: function (value) {
                if (value.toString().trim() == "") {
                    return '必填不能为空！';
                }
                dept2 = $('#gss2').val();
            }
            , ytu: function (value) {
                if (value.toString().trim() == "") {
                    return '必填不能为空！';
                }
                ytu = value.toString().trim();
            }
            , money: function (value) {
                if (value.toString().trim() == "") {
                    return '必填不能为空！';
                }
                if (value != null || value.toString().trim() != "") {
                    if (isNaN(value.toString().trim())) {
                        return '金额格式不对！';
                    }
                }
                money = value.toString().trim();
                if (flag == 1) {
                    date4 = $('#date4').val();
                } else if (flag == 2) {
                    date4 = $('#date42').val();
                }

            }
            , manl: function (value) {
                if (value.toString().trim() == "") {
                    return '必填不能为空！';
                } else {
                    $.ajax({
                        async: false,//ajax先执行，默认为ture
                        url: "/Financial/CheckName",
                        data: { Name: value.trim(), num: 5 },
                        type: "post",
                        dataType: "text",
                        success: function (data) {
                            if (data == "False") {
                                $('#flag5').val('False');
                            }
                            if (data == "True") {
                                $('#flag5').val('True');
                            }
                        }
                    });
                }
                if ($('#flag5').val() != 'False') {
                    return '领用人不存在！';
                }
                manl = value.toString().trim();
            }
            , manb: function (value) {
                if (value.toString().trim() == "") {
                    return '必填不能为空！';
                } else {
                    $.ajax({
                        async: false,//ajax先执行，默认为ture
                        url: "/Financial/CheckName",
                        data: { Name: value.trim(), num: 5 },
                        type: "post",
                        dataType: "text",
                        success: function (data) {
                            if (data == "False") {
                                $('#flag5').val('False');
                            }
                            if (data == "True") {
                                $('#flag5').val('True');
                            }
                        }
                    });
                    if ($('#flag5').val() != 'False') {
                        return '报销人不存在！';
                    }
                    manb = value.toString().trim();
                    if (flag == 1) {
                        mans = $('input[name= mans]').val();
                        manf = $('input[name= manf]').val();
                        note = $('textarea[name= note]').val();
                    } else if (flag == 2) {
                        mans = $('input[name= mans2]').val();
                        manf = $('input[name= manf2]').val();
                        note = $('textarea[name= note2]').val();
                    }

                    if (flag == 1) {
                        //新增凭证录入
                        $.ajax({
                            url: "/Financial/AddLaimingpz",
                            data: {
                                ytu: ytu, mans: mans, manf: manf,
                                manb: manb, manl: manl, money: money,
                                note: note, date4: date4, dept: dept2
                            },
                            type: "post",
                            dataType: "text",
                            success: function (data) {
                                if (data == "True") {
                                    layer.msg('新增成功！');
                                    table2.reload({
                                        where: {
                                            dates: datee
                                            , datee: dates,
                                            status: $('#gssz').val()

                                        }
                                        , page: {
                                            curr: 1
                                        }
                                    });
                                }
                                if (data == "False") {
                                    layer.msg('新增失败！');
                                }
                                layer.close(index2);
                            }
                        });
                    } else if (flag == 2) {
                        //修改报销单
                        $.ajax({
                            url: "/Financial/UpLaimingpzt",
                            data: {
                                id: id, ytu: ytu, mans: mans, manf: manf,
                                manb: manb, manl: manl, money: money,
                                note: note, date4: date4, dept: dept2
                            },
                            type: "post",
                            dataType: "text",
                            success: function (data) {
                                if (data == "True") {

                                    layer.msg('修改成功！');
                                    table2.reload({
                                        where: {
                                            dates: datee
                                            , datee: dates,
                                            status: $('#gssz').val()

                                        }
                                        , page: {
                                            curr: 1
                                        }
                                    });
                                }
                                if (data == "False") {
                                    layer.msg('修改失败！');
                                }
                                layer.close(index3);
                                form.render();
                            }
                        });
                    }

                }
            }
        });

        var objData = {}
        table.on('tool(demo)', function (obj) { //注：tool 是工具条事件名，test 是 table 原始容器的属性 lay-filter="对应的值"
            var data = obj.data //获得当前行数据
                , layEvent = obj.event; //获得 lay-event 对应的值
            id = data.编号, ytu = data.用途, mans = data.审批, dept2 = data.报销部门,
                money = data.金额, date4 = data.时间, manl = data.领用人, manb = data.报销人, danh = data.单号,
                manf = data.报销部门主管复核, note = data.备注, ztai = data.状态,lrzt=data.录入状态;
            name2 = name;
            if (layEvent === 'detail') {
                if (lrzt == '已记账') {
                    layer.msg('不可重复记账！');
                } else {
                    select.render({
                        '#gss5': { data: "/Financial/SeAccountingListpz", value: "编号", text: "科目名称" }
                    });
                    index5 = layer.open({
                        type: 1
                        , title: '记账'
                        , area: ['45%', '50%']
                        , content: $("#for5")
                        , skin: ['layui-layer-molv']
                    });
                }           

            } else if (layEvent === 'upload') {              
                var num = 0;
                $.ajax({
                    url: "/Financial/SeLaiminlj",
                    data: { id: danh, type: 'check2' },
                    type: "post",
                    dataType: "text",
                    success: function (data) {
                        if (data != null) {
                            var dataObj = eval("(" + data + ")");//转换为json对象 
                            $.each(dataObj, function (name, value) {
                                //alert(dataObj[name].col);
                                $('#demo3').append(' 附件' + (++num) + ':'
                                    + '<img src="/Uploadf/'
                                    + dataObj[name].col + '" alt="' + dataObj[name].col + '" title="' + dataObj[name].col
                                    + '+" class="layui-upload-img imgt"  style="width:100px;height:100px;">')

                            });
                            form.render();
                        }
                    }
                });

                index4 = layer.open({
                    type: 1
                    , title: '支付附件'
                    , cancel: function () {
                        // 你点击右上角 X 取消后要做什么

                        while (div2.hasChildNodes()) {
                            div2.removeChild(div2.firstChild);
                        }
                    }
                    , area: ['80%', '75%']
                    , content: $("#for3")
                    , skin: ['layui-layer-molv']
                });
            } else if (layEvent === 'del') {
                if (lrzt == '已记账') {
                    layer.msg('已记账不可删除！');
                } else {
                    layer.confirm('真的删除行么？', function (index) {
                        $.ajax({
                            url: "/Financial/DelLaiming",
                            data: { id: id },
                            type: "post",
                            dataType: "text",
                            success: function (data) {
                                if (data == "True") {
                                    layer.msg('删除成功！');
                                } else {
                                    layer.msg('删除失败！');
                                }
                            }
                        });
                        obj.del(); //删除对应行（tr）的DOM结构
                        layer.close(index);
                        //向服务端发送删除指令
                    });
                }

            } else if (layEvent === 'edit') {
                if (lrzt == '已记账') {
                    layer.msg('已记账不可修改！');
                } else {
                    flag = 2;
                    select.render({
                        '#gss2': { data: "/Financial/SeDepts", value: "ID", text: "Dept", duf: dept2 }
                    }, function (data) {
                        layui.each(data["#gss2"].data, function (index, value) {
                            objData[value.ID] = value;
                        })
                    });

                    $('input[name= mans2]').val(mans);
                    $('input[name= manf2]').val(manf);
                    $('input[name= ytu2]').val(ytu);
                    $('input[name= money2]').val(money);
                    $('input[name= date42]').val(date4);
                    $('input[name= manl2]').val(manl);
                    $('input[name= manb2]').val(manb);
                    $('textarea[name= note2]').val(note);
                    index3 = layer.open({
                        type: 1
                        , title: '修改报销单'
                        , area: ['50%', '93%']
                        , content: $("#fo2r")
                        , skin: ['layui-layer-molv']
                    });
                }
            } else if (layEvent === 'fs') {
                if (lrzt == '已记账') {
                    layer.confirm('真的反审么？', function (index) {
                        $.ajax({
                            url: "/Financial/UpLaimingpzfs",
                            data: { id: id },
                            type: "post",
                            dataType: "text",
                            success: function (data) {
                                if (data == "True") {
                                    layer.msg('反审成功！');
                                } else {
                                    layer.msg('反审失败！');
                                }
                                table2.reload({
                                    where: {
                                        dates: datee
                                        , datee: dates,
                                        status: $('#gssz').val()

                                    }
                                    , page: {
                                        curr: 1
                                    }
                                });
                                form.render();
                            }
                        });
                        layer.close(index);
                    });
                } else {
                    layer.msg('未记账不可反审！');
                }
            }
        });

        var div2 = document.getElementById("demo3");
        var files, msg, code;
        var indexs = [];
        var boo = false;
        var a = '';
        var nus = 1;

        //附件上传取消
        $("#qx3").on('click', function () {

            num = 0;
            boo = false;

            while (div2.hasChildNodes()) {
                div2.removeChild(div2.firstChild);
            }
            layer.close(index4);
        });

        //审核
        $("#shenhe").on('click', function () {
            var checkStatus = table.checkStatus("tw9")
                , data = checkStatus.data;
            var delList = [];
            var delList2 = [];
            var delList3 = [];
            len = data.length;
            var a = false;
            data.forEach(function (n, i) {
               
                delList.push(n.编号);
                if (n.zt == '已') {
                    a = true;
                }

            })
            if (len == 0) {
                layer.msg('请选择一行！');
            } else if (a) {
                layer.msg('请选择未审核行！');
            } else {
                //修改报销单
                //$.ajax({
                //    url: "/Financial/UpLaimingsh",
                //    data: {
                //        delList: delList
                //        , name: name
                //    },
                //    type: "post",
                //    dataType: "text",
                //    success: function (data) {
                //        if (data == "True") {
                //            layer.msg('审核成功！');
                //            table2.reload({
                //                where: {
                //                    dates: dates
                //                    , datee: datee
                //                    , name: name
                //                    , status: $('#gssz').val()

                //                }
                //                , page: {
                //                    curr: 1
                //                }
                //            });
                //            layer.close(index3);
                //            form.render();
                //        }
                //        if (data == "False") {
                //            layer.msg('审核失败！');
                //            layer.close(index3);
                //            form.render();
                //        }

                //    }
                //});
            }
        })

    });
})