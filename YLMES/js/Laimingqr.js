
    layui.config({
        base: '/js/'
    }
    ).use(['upload', 'laydate', 'table', 'layer', 'form', 'Select'], function () {
        var table = layui.table, layer = layui.layer, form = layui.form, laydate = layui.laydate, upload = layui.upload,$=layui.jquery;
        var dates = $("#dates").val();
        var datee = $("#datee").val();
        var select = layui.Select

        var index5 ;
        var index4 ;

        var table2 ;
        var table3;
   
        select.render({
            '.gss8': { data: "/Financial/SeCaptionall", value: "name", text: "name" }
        });
        select.render({
            '.gssb': { data: "/Financial/SeDepts", value: "编号", text: "部门" }
        });
        var a = 1;

        //记字号初始赋值
        laydate.render({
            elem: '#jrq'
            , value: s3
            , isInitValue: true
            , theme: 'molv'
            , done: function (value, date, endDate) {

                $.ajax({
                    async: false,//ajax先执行，默认为ture
                    url: "/Financial/SeAccuntingJZH",
                    data: {
                        date: value
                    },
                    type: "post",
                    dataType: "text",
                    success: function (data) {
                        if (data != null) {
                            var dataObj = eval("(" + data + ")");//转换为json对象 
                            $.each(dataObj, function (name, value) {
                                //alert(dataObj[name].记字号);
                                $('#jzh2').val(dataObj[name].记字号);
                                $('#lb8').text('记字号：' + dataObj[name].记字号);
                            });
                        }
                        form.render();
                    }
                });
            }
        });
        var val
        var val1
        //按等于号 实现凭证借贷平衡
        var key=function () {
            if (event.code == "Equal") { // 按 =号平衡   
                try {
                    val = Number($(".layui-table-total .laytable-cell-" + a + "-0-4").text())
                    val1 = Number($(".layui-table-total .laytable-cell-" + a + "-0-5").text())
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
                    tt.reload({
                        data: table.cache.myBtn
                    })
                } catch(a)
                   {


                }
            }
        }
        var key2 = function () {
            var boo = false;
            $(this).parents("tr").prev().find(".layui-table-cell").each(function () {
               
                $(this).text() == "" || 0 ? 
                    boo = true
                  : 
                    boo = false
                 
                if (boo) {
                    return false;
                }
            })
            if (boo) {
                layer.msg("请填写完整上一行")
                tt.reload({
                    data: table.cache.myBtn
                })
            }
        }
        $(document).on("keydown", 'td[data-field="name2"],td[data-field="name3"],td[data-field="name"] ', key2);//键盘事件
        $(document).on("keydown", 'tbody td[data-field="name2"],td[data-field="name3"]', key);//键盘事件

        var today2 = new Date();
       var s3 =  today2.getFullYear() + "-" + (today2.getMonth()+1) + "-" + today2.getDate();
      
        table2 = table.render({
            elem: '#tw'
            , url: '/Financial/SeAccuntingListpz'
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
                , { field: '项目编号2', width: 150, title: '项目编号' }
                , { field: '制单人', width: 150, title: '制单人' }
                , { field: '记字号', width: 150, title: '记字号' }
                , { field: '时间', width: 150, title: '凭证日期' }
                , { field: '状态', width: 150, title: '状态' }
                , { field: 'right', width: 280, align: 'center', title: '操作', toolbar: '#barDemo2' }
            ]]
        });

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
            index2= layer.open({
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
                , done: function () {
                    $("tr a").hide();
                    $("tr").hover(function () {
                        $(this).children().last().children().children().toggle();
                    });
                }
                , cols: [[
                    { type: 'checkbox', fixed: 'left' }
                    , { field: '编号', width: 60, fixed: 'left', title: '序号' }
                    , { field: '部门', width: 150, title: '费用部门' }
                    , { field: '单号', width: 170, title: '单号(查看)'}
                    , { field: '用途', width: 150, title: '用途', style: "color: #009688" }
                    , { field: '金额', width: 150, title: '金额' }
                    , { field: '支付时间2', width: 150, title: '支付时间2'}
                    , { field: '附件数量', width: 150, title: '附件数量' }
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

        //新增    
        var tt
      
        var s2;
        var aa;
        
        //保存
        $('#b2c').on('click', function () {
            var bo = false
            val = Number($(".layui-table-total .laytable-cell-" + a + "-0-4").text())
            val1 = Number($(".layui-table-total .laytable-cell-" + a + "-0-5").text())
            
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
                    if (aa != "" && aa != undefined && aa != null) {
                        table.cache.myBtn.forEach(function (s, i) {
                            $.post("/Financial/AccuntingAD", { datas: s, jzdh: s2, jzh: $('#jzh2').val(), jrq: $("#jrq").val(), xm: aa, index: i }, function (data) {

                            })
                        })
                        layer.msg("保存成功")
                    } else {
                        layer.msg("请选择项目编号")
                    }
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
            $(".gssb").each(function () {
                if ($(this).val() == '') {
                    ff = false;
                    return;
                }
            });
            var data2 = table.cache.myBtn;
            data2.forEach(function (n, i) {
                if (n.name == '' || (n.name2 == '0.00' && n.name3 == '0.00')) {
                    ff = false;
                    return;
                }
            })
            if (!ff) {
                layer.msg('请填写完整凭证！');
            } else {
                table.cache.myBtn.push({
                    name2: '0.00',
                    name3: '0.00'
                })
                tt.reload({
                    data: table.cache.myBtn
                })
            }
           
        }) 
        //新增 
        $('#adds').on('click', function () {
            index6 = layer.open({
                type: 1
                , title: '新增凭证'
                , area: ['100%', '100%']
                , content: $("#for7")
                , skin: ['layui-layer-molv']
                , success: function () {
                    a++;
                        tt = table.render({
                            elem: '#myBtn'
                            , limit: 15
                            , height: 'full-300'
                            , totalRow: true
                            , done: function (res) {
                                $("tr a").hide();
                                $("tr").hover(function () {
                                    $(this).children().last().children().children().toggle();
                                });
                                select.reload(res.data, { "gss8": "name1", "gssb": "name4" }, "#gss6")
                            }
                            , cols: [[
                                { type: "numbers", title: '序号', totalRowText: '合计' }
                                , { field: 'name', title: '摘要', edit: 'text' }
                                , { field: 'name1', templet: '#jie', title: '科目名称' }
                                , { field: 'name4', templet: '#jie2', title: '部门' }
                                , { field: 'name2', title: '借方', event: 'a', edit: 'text', totalRow: true }
                                , { field: 'name3', edit: 'text', title: '贷方', sort: true, totalRow: true }
                                , { field: 'right', align: 'center', title: '操作', toolbar: '#barDemo3' }

                            ]],
                            data: [{ name2: "0.00", name3: "0.00" }, { name2: "0.00", name3: "0.00" }]
                        })
                }
            });
            //初始赋值
            laydate.render({
                elem: '#jrq'
                , value: s3
                , isInitValue: true
                , theme: 'molv'
   
            });
            //加载记字号
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
                            //alert(dataObj[name].记字号);
                            $('#jzh2').val(dataObj[name].记字号);
                            $('#lb8').text('记字号：' + dataObj[name].记字号);

                        });
                    }

                    form.render();
                }
            });
            select.render({
                '#gss6': { data: "/Financial/SeContract", value: "id", text: "name" }
            });
            today = new Date();
            s2 = 'JD' + today.getFullYear() + "" + today.getMonth() + "" + today.getDate() + "" + today.getHours() + "" + today.getMinutes() + "" + today.getSeconds();
            //$('input[name=jzdh]').val(s2);
            $('#lb5').val();
            $('#lb5').text('记账单号：' + s2);
            //$('#lb6').text('日期：' + data.时间);
            $('#lb7').text('制单人：' + $('#Sessioname').val());
        
        })
     
        //新增 
        $('#add2').on('click', function () {
        
            var t = false;
            var checkStatus = table.checkStatus("tw9")
                , data = checkStatus.data;
            data.forEach(function (n, i) {
                if (n.录入状态=='已记账') {
                    t = true;
                }
            })
            if (!t) {
               index6= layer.open({
                    type: 1
                    , title: '记账凭证'
                    , area: ['100%', '100%']
                    , content: $("#for7")
                   , skin: ['layui-layer-molv']
                   , cancel: function () {
                       // 你点击右上角 X 取消后要做什么
                      
                       table2.reload({
                           where: {
                               dates: dates
                               , datee: datee
                           }
                           , page: {
                               curr: 1
                           }
                       });
                   },
                    success: function () {
                         table.render({
                            elem: '#myBtn'
                            , limit: 15
                            , totalRow: true
                            , done: function (res) {
                                $("tr a").hide();
                                $("tr").hover(function () {
                                    $(this).children().last().children().children().toggle();
                                });
                                $("td[data-field='name1']").each(function () {
                                    $(this).find(".layui-table-cell").removeClass('layui-table-cell');
                                });      
                                select.render({
                                    '.gss8': { data: "/Financial/SeCaptionall", value: "name", text: "name" }
                                });
                                select.render({
                                    '.gssb': { data: "/Financial/SeDepts", value: "编号", text: "部门" }
                                });
                            }
                            , cols: [[
                                  { type: "numbers", title: '序号9', totalRowText: '合计' }
                                , { field: 'name', title: '摘要', width: 120, edit: 'text' }
                                , { field: 'name1', templet: '#jie', title: '科目名称5', style: "" }                         
                                , { field: 'name2', title: '借方', edit: 'text',totalRow: true }
                                , { field: 'name3', edit: 'text', title: '贷方', totalRow: true }
                                , { field: 'name4', templet: '#jie2', title: '部门', style: "" }
                                , { field: 'right', width: 185, align: 'center', title: '操作', toolbar: '#barDemo3' }

                            ]],
                            data: []
                        });
                    }
                });
            
                select.render({
                    '#gss6': { data: "/Financial/SeContract", value: "id", text: "name" }
                });
              
                form.render();
            } else {
                layer.msg('请选择未记账行！');
            }
     
        })
        //单元格监听
        table.on('edit(myBtn)', function (obj) {
            var t = obj.value
            var ind = $("tbody tr").index(obj.tr)
            if (obj.field == 'name2' || obj.field == 'name3') {
                if (isNaN(obj.value)) {
                    layer.msg("只可以输入数字", { icon: 5 })
                    t = 0;
                }
                if (obj.field == 'name2') {
                    table.cache.myBtn[ind]['name3'] = Number(0).toFixed(2)
                } else if (obj.field == 'name3') {
                    
                    table.cache.myBtn[ind]['name2'] = Number(0).toFixed(2)
                }
                table.cache.myBtn[ind][obj.field] = Number(t).toFixed(2)
                tt.reload({
                    url: "",
                    data: table.cache.myBtn
                })
            }
        });
        form.on('select', function (obj) {
            if ($(obj.elem).parents("table").attr("class") != undefined) {
                var ind = $("tbody tr").index($(obj.elem).parents("tr"))
                var n = $(obj.elem).parents("td").attr("data-field")
                table.cache.myBtn[ind][n] = obj.value
            } else {
                aa = obj.value
            }
        })
        //附件上传取消
        $("#qx3").on('click', function () {

            num = 0;
            boo = false;

            while (div2.hasChildNodes()) {
                div2.removeChild(div2.firstChild);
            }
            layer.close(index4);
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
        //模糊查询
        $("#Search3").on('click', function () {
            var dates = $("#dates3").val();
            var datee = $("#datee3").val();
            var status = $("#gssz3").val();
            table3.reload({
                where: {
                    dates: dates
                    , datee: datee
                    ,status:status
                }
                , page: {
                    curr: 1
                }
            });
            form.render();
        });

        //记账取消
        $("#qx5").on('click', function () {
            layer.close(index5);
        });

        table.on('tool(myBtn)', function (obj) {
           var layEvent = obj.event; //获得 lay-event 对应的值
            if (layEvent === 'del') {
                //删除
                obj.del(); //删除对应行（tr）的DOM结构
                 
                tt.reload({
                    data: table.cache.myBtn
                })
            }
        })
        var objData = {}
        table.on('tool(demo)', function (obj) { //注：tool 是工具条事件名，test 是 table 原始容器的属性 lay-filter="对应的值"
            var data = obj.data //获得当前行数据
                , layEvent = obj.event; //获得 lay-event 对应的值
            id = data.编号, ytu = data.用途, mans = data.审批, dept2 = data.报销部门,
                money = data.金额, date4 = data.时间, manl = data.领用人, manb = data.报销人, danh = data.单号,
                manf = data.报销部门主管复核, note = data.备注, ztai = data.状态, lrzt = data.录入状态,danh2=data.凭证单号;
            name2 = name;
            if (layEvent === 'detail') {

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

            } else if (layEvent === 'check') {
                index7=layer.open({
                    type: 1
                    , title: '记账凭证'
                    , area: ['75%', '70%']
                    , content: $("#for9")
                    , skin: ['layui-layer-molv']
                });
                $('#lb').text('单位名称：友力智能');
                $('#lb2').text('日期：' + data.时间);
                $('#lb3').text('记字号：' + data.记字号);
                $('#lb4').text('制单人：' + data.制单人);
                table4 = table.render({
                    elem: '#tw1'
                    , url: '/Financial/SeAccuntingse?danh=' + danh2
                    , totalRow: true
                    , limit: 15
                    , cols: [[
                        { field: '', hide: true }
                           ,{
                            type: "numbers", title: '序号'
                        }
                        //, { field: '序号', width: 60, fixed: 'left', title: '序号' }
                        , { field: '摘要', title: '摘要',  totalRowText: '合计'}
                        , { field: '科目名称', title: '科目名称' }
                        , { field: '借方', width: 185, align: 'center',  title: '借方', totalRow: true}
                        , { field: '贷方', width: 185, align: 'center',  title: '贷方', totalRow: true}
      
                    ]]
                });
            } else if (layEvent === 'del') {
                //删除
                obj.del(); //删除对应行（tr）的DOM结构
                tt.reload({
                    data: table.cache.myBtn
                })
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
                    layer.close(index);
                });
            } else if (layEvent == 'xi') {
                layer.open({
                    type: 1
                    , title: '单号查看'
                    , area: ['100%', '100%']
                    , content: $("#for5")
                    , skin: ['layui-layer-molv']
                });
                table3 = table.render({
                    elem: '#tw9'
                    , url: '/Financial/SeLaimingListDanh?danh=' + danh
                    , page: true
                    , limit: 15
                    , cols: [[
                        { field: '', hide: true }
                        , { field: '编号', width: 60, fixed: 'left', title: '序号' }
                        , { field: '费用部门', width: 150, title: '费用部门' }
                        , { field: '单号', width: 170, title: '单号(查看)' }
                        , { field: '用途', width: 150, title: '用途', style: "color: #009688" }
                        , { field: '金额', width: 150, title: '金额' }
                        , { field: '时间', width: 150, title: '时间' }
                        , { field: '附件数量', width: 150, title: '附件数量' }
                        , { field: '领用人', width: 150, title: '领用人' }
                        , { field: '报销人', width: 150, title: '报销人' }
                        , { field: '报销部门主管复核', width: 150, title: '报销部门主管复核' }

                        , { field: '状态', width: 150, title: '状态' }
                        , { field: '备注', width: 150, title: '备注' }
                    ]]
                });

            }
        });
    })