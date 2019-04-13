$(function () {
    layui.use(['table', 'layer', 'form'], function () {
        var table = layui.table, layer = layui.layer, form = layui.form;
        var NameSearch = $("#Name").val().trim();
        var table2 = table.render({
            elem: '#tw'
            , url: '/Financial/SeAccountingList?Name=' + NameSearch
            , page: true
            , limit: 15
            , done: function () {
                $("tr a").hide();
                $("tr").hover(function () {
                    $(this).children().last().children().children().toggle();
                });
            }
            , cols: [[
                { filed: '', hide: true }
                , { field: '编号', width: 60, fixed: 'left', title: '序号' }
                , { field: '科目代码', width: 150, title: '科目代码'}
                , { field: '科目名称', width: 150, title: '科目名称', style: "color: #009688"  }
                , { field: '科目类别', width: 150, title: '科目类别' }
                , { field: '余额方向', width: 150, title: '余额方向' }
                , { field: '全名', width: 150, title: '全名' }
                , { field: '现金科目', width: 150, title: '现金科目' }
                , { field: '银行科目', width: 150, title: '银行科目' }
                , { field: '项目辅助核算', width: 150, title: '项目辅助核算' }
                , { field: '状态', width: 150, title: '状态' }
                , { field: 'right', width: 178, align: 'center', title: '操作', toolbar: '#barDemo' }
            ]]
        });
        var index2 = Object;
        var index3 = Object;
        var name2 = '', status = '', id = '',codex='';
        //模糊查询
        $("#Search").click(function () {
            var NameSearch = $("#Name").val().trim();
            table2 = table.render({
                elem: '#tw'
                , url: '/Financial/SeAccountingList?Name=' + NameSearch
                , page: true
                , limit: 15
                , done: function () {
                    $("tr a").hide();
                    $("tr").hover(function () {
                        $(this).children().last().children().children().toggle();
                    });
                }
                , cols: [[
                    { filed: '', hide: true }
                    , { field: '编号', width: 60, title: '序号' }
                    , { field: '科目代码', width: 150, title: '科目代码'}
                    , { field: '科目名称', width: 150, title: '科目名称', style: "color: #009688"  }
                    , { field: '科目类别', width: 150, title: '科目类别' }
                    , { field: '余额方向', width: 150, title: '余额方向' }
                    , { field: '全名', width: 150, title: '全名' }
                    , { field: '现金科目', width: 150, title: '现金科目' }
                    , { field: '银行科目', width: 150, title: '银行科目' }
                    , { field: '项目辅助核算', width: 150, title: '项目辅助核算' }
                    , { field: '状态', width: 150, title: '状态' }
                    , { field: 'right', width: 178, align: 'center', title: '操作', toolbar: '#barDemo' }
                ]]
            });
        });
        //新增
        $('#add').on('click', function () {
            $.ajax({
                async: false,//ajax先执行，默认为ture
                url: "/Financial/SeSubjectTypeLists",
                type: "post",
                dataType: "json",
                success: function (data) {
                    if (data != null) {
                        for (var i = 0; i < data.length; i++) {
                            $("#gss").append('<option value="' + data[i].编号 + '">' + data[i].科目类别 + '</option>');
                            form.render()
                        }
                        //$.each(data, function (i, result) {
                        //    alert(result.deptNo);
                        //})
                    }
                    else {
                        layer.msg('加载失败！');
                    }
                }
           });
            index2 = layer.open({
                type: 1
                , title: '新增科目类别'
                , area: ['50%', '90%']
                , content: $("#for")
                , skin: ['layui-layer-molv']
            });
        });
        //新增取消
        $("#qx").on('click', function () {
            layer.close(index2);
        });
        //修改取消
        $("#qx2").on('click', function () {
            layer.close(index3);
        });
        //自定义验证规则--新增科目类别form表单验证
        form.verify({
            SubjectCode : function (value) {
                $.ajax({
                    async: false,//ajax先执行，默认为ture
                    url: "/Financial/CheckName",
                    data: { Name: value.trim(), num: 2 },
                    type: "post",
                    dataType: "text",
                    success: function (data) {
                        if (data == "False") {
                            $('#flag').val('False');
                        }
                        if (data == "True") {
                            $('#flag').val('True');
                        }
                    }
                });
                if (value.toString().trim() == "") {
                    return '必填不能为空！';
                } else if ($('#flag').val() == 'False') {
                    return '科目代码已存在！';
                }
            }
            , SubjectCode2: function (value) {
                if (value.trim() != codex.trim()) {
                   
                    $.ajax({
                        async: false,//ajax先执行，默认为ture
                        url: "/Financial/CheckName",
                        data: { Name: value.trim(), num: 2 },
                        type: "post",
                        dataType: "text",
                        success: function (data) {
                           
                            if (data == "False") {
                                $('#flag3').val('False');
                            }
                            if (data == "True") {
                                $('#flag3').val('True');
                            }
                        }
                    });
                }
  
                if (value.toString().trim() == "") {
                    return '必填不能为空！';
                } else if ($('#flag3').val() == 'False' && value.trim() != codex.trim()) {
                    return '科目代码已存在！';
                }
            }
            , SubjectTypeName : function (value) {
                $.ajax({
                    async: false,//ajax先执行，默认为ture
                    url: "/Financial/CheckName",
                    data: { Name: value.trim(), num: 3 },
                    type: "post",
                    dataType: "text",
                    success: function (data) {
                        if (data == "False") {
                            $('#flag').val('False');
                        }
                        if (data == "True") {
                            $('#flag').val('True');
                        }
                    }
                });
                if (value.toString().trim() == "") {
                    return '必填不能为空！';
                } else if ($('#flag').val() == 'False') {
                    return '科目名称已存在！';
                } else {                
                    var SubjectCode = $('input[name= SubjectCode]').val().toString().trim();
                    var SubjectTypeName = $('input[name= SubjectTypeName]').val().toString().trim();
                    var code2 = $('input[name= code2]').val().toString().trim(); 
                    var gss = $('#gss').val().toString().trim();
                    var sexa = $('input[name="sexa"]:checked ').val().toString().trim(); 
                    var AllName = $('input[name= AllName]').val().toString().trim();
                    var sexb = $('input[name="sexb"]:checked ').val().toString().trim();
                    var sexd = $('input[name="sexd"]:checked ').val().toString().trim();
                    var fu = $('input[name= fu]').val().toString().trim();
                    var sexd = $('input[name="sexe"]:checked ').val().toString().trim();

                    //新增科目
                    $.ajax({
                        url: "/Financial/AddAccounting",
                        data: {
                            SubjectCode : SubjectCode, SubjectTypeName: SubjectTypeName,
                            code2: code2, gss: gss, sexa: sexa, AllName: AllName,
                            sexb: sexb, sexd: sexd, fu: fu
                        },
                        type: "post",
                        dataType: "text",
                        success: function (data) {
                            if (data == "True") {   
                                layer.msg('新增成功！');
                                table2.reload({
                                    page: {
                                        curr: 1
                                    }
                                }); 
                                layer.close(index2);

                            }
                            if (data == "False") {
                                layer.close(index2);
                                layer.msg('新增失败！');
                            }
                            layer.close(index2);
                        }
                    });
                }
            }
            , SubjectTypeName2: function (value) {
                if (value.toString().trim() == "") {
                    return '必填不能为空！';
                }
                if (value.trim() != name2.trim()) {
                    $.ajax({
                        async: false,//ajax先执行，默认为ture
                        url: "/Financial/CheckName",
                        data: { Name: value.trim(), num: 3 },
                        type: "post",
                        dataType: "text",
                        success: function (data) {
                            if (data == "False") {
                                $('#flag2').val('False');
                            }
                            if (data == "True") {
                                $('#flag2').val('True');
                            }
                        }
                    });
                }
                if ($('#flag2').val() == 'False' && value.trim() != name2.trim()) {
                    return '科目类别名称已存在！';
                } else {
                    var SubjectCode = $('input[name= SubjectCode2]').val().toString().trim();
                    var SubjectTypeName = $('input[name= SubjectTypeName2]').val().toString().trim();
                    var code2 = $('input[name= code3]').val().toString().trim();
                    var gss = $('#gss2').val().toString().trim();
                    var sexa = $('input[name="sexa2"]:checked ').val().toString().trim();
                    var AllName = $('input[name= AllName2]').val().toString().trim();
                    var sexb = $('input[name="sexb2"]:checked ').val().toString().trim();
                    var sexd = $('input[name="sexd2"]:checked ').val().toString().trim();
                    var fu = $('input[name= fu2]').val().toString().trim();
                    var status = $('input[name="sexe"]:checked ').val().toString().trim();
                    form.render();
                    $.ajax({
                        url: "/Financial/UpAccounting",
                        data: {
                            SubjectCode: SubjectCode, SubjectTypeName: SubjectTypeName,
                            code2: code2, gss: gss, sexa: sexa, AllName: AllName,
                            sexb: sexb, sexd: sexd, fu: fu, id: id, status: status
                        },
                        type: "post",
                        dataType: "text",
                        success: function (data) {
                            if (data == "True") {
                                layer.close(index3); 
                                layer.msg('修改成功！'); 
                                table2.reload({
                                    page: {
                                        curr: 1
                                    }
                                });
                                form.render();                      
                            } else {
                                layer.close(index3);
                                layer.msg('修改失败！');
                            }
                            layer.close(index3);
                        }
                    });
                }
            }
        });
        table.on('tool(demo)', function (obj) { //注：tool 是工具条事件名，test 是 table 原始容器的属性 lay-filter="对应的值"
            var data = obj.data //获得当前行数据
                , layEvent = obj.event; //获得 lay-event 对应的值
            id = data.编号, SubjectCode = data.科目代码, status = data.状态, SubjectTypeName = data.科目名称,
                code2 = data.助记码, gss = data.科目类别, sexa = data.余额方向, AllName = data.全名,
                sexb = data.现金科目, sexd = data.银行科目, fu = data.项目辅助核算;
            name2 = data.科目名称, codex = data.科目代码;
           
            if (layEvent === 'detail') {
                layer.msg('查看操作');
            } else if (layEvent === 'del') {
                layer.confirm('真的删除行么？', function (index) {
                    $.ajax({
                        url: "/Financial/DlAccounting",
                        data: { ID: id },
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
            } else if (layEvent === 'edit') {
                $.ajax({
                    async: false,//ajax先执行，默认为ture
                    url: "/Financial/SeSubjectTypeLists",
                    type: "post",
                    dataType: "json",
                    success: function (data) {
                        if (data != null) {
                            for (var i = 0; i < data.length; i++) {
                                $("#gss2").append('<option value="' + data[i].编号 + '">' + data[i].科目类别 + '</option>');
                                form.render()
                            }
                        }
                        else {
                            layer.msg('加载失败！');
                        }
                    }
                });
                $('input[name= SubjectCode2]').val(SubjectCode);
                $('input[name= SubjectTypeName2]').val(SubjectTypeName);
                $('input[name= code3]').val(code2);
                $('#gss2').val(gss);
                $('input[name= AllName2]').val(AllName);
                $('input[name= fu2]').val(fu);
                if (sexa == '借') {
                    $("#sexa2").prop("checked", true);
                    form.render();
                } else {
                    $("#sexa2").prop("checked", true);
                    form.render();
                }
                if (sexb == '是') {
                    $("#sexb2").prop("checked", true);
                    form.render();
                } else {
                    $("#sexb2").prop("checked", true);
                    form.render();
                }
                if (sexd == '是') {
                    $("#sexd2").prop("checked", true);
                    form.render();
                } else {
                    $("#sexd2").prop("checked", true);
                    form.render();
                }
                if (status == '有效') {
                    $("#sexe").prop("checked", true);
                    form.render();
                } else {
                    $("#sexe").prop("checked", true);
                    form.render();
                }
                index3 = layer.open({
                    type: 1
                    , title: '修改科目'
                    , area: ['50%', '93%']
                    , content: $("#for2")
                    , skin: ['layui-layer-molv']
                });
            }
        });

    });
})