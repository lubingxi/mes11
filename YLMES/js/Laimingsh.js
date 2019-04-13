$(function () {
    layui.config({
        base: '/js/'
    }
    ).use(['upload', 'laydate', 'table', 'layer', 'form', 'Select'], function () {
        var table = layui.table, layer = layui.layer, form = layui.form, laydate = layui.laydate, upload = layui.upload;
        var dates = $("#dates").val();
        var datee = $("#datee").val();
       
        var select = layui.Select

        var index3 = Object, index4 = Object;
        var table3 = Object;
        var name = $('#Sessioname').val(), dept = '';
        var table2 = Object;
        var ers = '';
        var danh = '';
        var deptf = '';
        var len = 0;
        //var sesionnameid = {};
        //if ($('#Sessioname').val() == '朱小梅') {
        //    ers = '1';
        //}
        //select.render({
        //    '#gssname': { data: "/Financial/SeDepts", value: "ID", text: "Dept" }
        //}, function (data) {
        //    layui.each(data["#gssname"].data, function (index, value) {
        //        if ($('#Sessioname').val() == value.Manager) {
        //            //sesionname = 1;
        //            sesionnameid[index] = value.ID
        //        }
        //        if (value.Dept == '采购部') {
               
        //            if ($('#Sessioname').val() == '朱小梅') {
        //                ers = '1';
        //                sesionnameid[index] = value.ID
        //            }
        //        }
            
        //    })
        //});       
        table2 = table.render({
                elem: '#tw'
                , url: '/Financial/SeLaimingListsh'
                , where: { name: name }
                , page: true
                , limit: 15
                , cols: [[
                    { type: 'checkbox', fixed: 'left' }
                    , { field: '编号', width: 60, fixed: 'left', title: '序号' }                 
                    , { field: '费用部门', width: 150, title: '费用部门' }
                    , { field: '单号', width: 170, title: '单号(查看)', event: 'xi', sort: true, templet: '#cc' } 
                    , { field: '用途', width: 150, title: '用途', style: "color: #009688" }
                    , { field: '金额', width: 150, title: '金额' }
                    , { field: '时间2', width: 150, title: '时间' }
                    , { field: '附件数量', width: 150, title: '附件数量' }
                    , { field: '领用人', width: 150, title: '领用人' }
                    , { field: '报销人', width: 150, title: '报销人' }
                    , { field: '报销部门主管复核', width: 150, title: '报销部门主管复核' }
                    //, { field: '审批', width: 150, title: '审批' }
                    //, { field: '状态', width: 150, title: '状态' }
                    , { field: '备注', width: 150, title: '备注' }
                    //, { field: '录入状态', width: 150, title: '录入状态' }
                    , { field: 'zt', width: 150, title: '状态', event: 'xi', sort: true, templet: '#cca' }
                    , { field: 'right', width: 185, align: 'center', title: '操作', toolbar: '#barDemo' }
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
             dates = $("#dates").val().trim();
            datee = $("#datee").val().trim();
            table2.reload({
                where: {
                    dates: dates
                    , datee: datee
                    , name: name
                    , status: $('#gssz').val()
                 
                }
                , page: {
                    curr: 1
                }
            });
            form.render();
        });
        //审核
        $("#shenhe").on('click', function () {
            var checkStatus = table.checkStatus("tw")
                , data = checkStatus.data;
            var delList = [];
            var delList2 = [];
            len = data.length;              
            var a = false;
            data.forEach(function (n, i) {
                delList.push(n.编号);
                delList2.push(n.单号);
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
                        $.ajax({
                            url: "/Financial/UpLaimingsh",
                            data: {
                                delList: delList
                                , delList2: delList2
                                , name: name                                                            
                            },
                            type: "post",
                            dataType: "text",
                            success: function (data) {
                                if (data == "True") {
                                    layer.msg('审核成功！');
                                    table2.reload({
                                        where: {
                                            dates: dates
                                            , datee: datee
                                            , name: name
                                            , status: $('#gssz').val()

                                        }
                                        , page: {
                                            curr: 1
                                        }
                                    });
                                    layer.close(index3);
                                    form.render();
                                }
                                if (data == "False") {
                                    layer.msg('审核失败！');
                                    layer.close(index3);
                                    form.render();
                                }
                               
                            }
                        });
            }
        })


        table.on('tool(demo)', function (obj) { //注：tool 是工具条事件名，test 是 table 原始容器的属性 lay-filter="对应的值"
            var data = obj.data //获得当前行数据
                , layEvent = obj.event; //获得 lay-event 对应的值
            id = data.编号, ytu = data.用途, mans = data.审批, dept2 = data.报销部门,
                money = data.金额, date4 = data.时间, manl = data.领用人, manb = data.报销人, bas = data.八级审核人,
                manf = data.报销部门主管复核, ztai = data.状态, lrzt = data.录入状态, danh = data.单号, zt = data.zt, zt2 = data.zt2,yis=data.一级审核人;
            name2 = name;
       
            if (layEvent === 'detail') {
                if (bas == $('#Sessioname').val() && zt2 != '已') {
                    zt2 == '待';
                }
                if (zt == '已' && zt2 =='待') {
                    layer.confirm('真的反审么？', function (index) {
                        $.ajax({
                            url: "/Financial/UpLaimingztaifs",
                            data: { id: id, dh: danh, name: $('#Sessioname').val() },
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
                                        dates: dates
                                        , datee: datee
                                        , name: name
                                        , status: $('#gssz').val()

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
                } else if (zt2 =='已') {
                    layer.msg('记录已更改不可反审！');
                  }else {
                    layer.msg('未审核不可反审！');
                }
            } else if (layEvent === 'upload') {
   
                var num = 0;
                $.ajax({
                    url: "/Financial/SeLaiminlj",
                    data: { id: id,type:'check' },
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
                    , title: '上传附件'
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

            } else if (layEvent === 'edit') {
   
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
                        
                         { field: '编号', width: 60, fixed: 'left', title: '序号' }
                        , { field: '费用部门', width: 150, title: '费用部门' }
                        , { field: '单号', width: 170, title: '单号(查看)'}
                        , { field: '用途', width: 150, title: '用途', style: "color: #009688" }
                        , { field: '金额', width: 150, title: '金额' }
                        , { field: '时间', width: 150, title: '时间' }
                        , { field: '附件数量', width: 150, title: '附件数量' }
                        , { field: '领用人', width: 150, title: '领用人' }
                        , { field: '报销人', width: 150, title: '报销人' }
                        , { field: '报销部门主管复核', width: 150, title: '报销部门主管复核' }
             
                        , { field: '备注', width: 150, title: '备注' }
                        //, { field: '状态', width: 150, title: '状态' }
                    ]]
                });

            }
        });

        //多图片上传

        var div2 = document.getElementById("demo3");
        //附件上传取消
        $("#qx3").on('click', function () {

            while (div2.hasChildNodes()) {
                div2.removeChild(div2.firstChild);
            }
            layer.close(index4);
        });

    });
})