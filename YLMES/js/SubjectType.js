$(function () {
    layui.use(['table', 'layer', 'form'], function () {
        var table = layui.table, layer = layui.layer, form = layui.form;;
        var NameSearch = $("#Name").val().trim();
        var table2=table.render({
            elem: '#tw'
            , url: '/Financial/SeSubjectTypeList?Name=' + NameSearch
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
                , { field: '编号', width: 50, fixed:'left', title: '序号' }
                , { field: '科目类别', width: 150, title: '科目类别', style: "color: #009688" }
                , { field: '状态', width: 150, title: '状态' }
                , { field: 'right', width: 178, align: 'center',title:'操作', toolbar: '#barDemo' }
            ]]
        });
        var index2 = Object;
        var index3 = Object;
        var name2 = '', status = '', id = '';
        $("#Search").click(function () {
            var NameSearch = $("#Name").val().trim();
             table2 =table.render({
                elem: '#tw'
                , url: '/Financial/SeSubjectTypeList?Name=' + NameSearch
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
                    , { field: '编号', width: 50, title: '序号' }
                    , { field: '科目类别', width: 150, title: '科目类别', style: "color: #009688" }
                    , { field: '状态', width: 150, title: '状态' }
                    , { field: 'right', width: 178, align: 'center', title: '操作', toolbar: '#barDemo' }
                ]]
            });
        });
        //新增
        $('#add').on('click', function () {
            index2 = layer.open({
                type: 1
                , title: '新增科目类别'
                , area: ['50%', '30%']
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
            SubjectTypeName: function (value) {
                $.ajax({
                    async: false,//ajax先执行，默认为ture
                    url: "/Financial/CheckName",
                    data: { Name: value.trim(), num: 1 },
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
                    return '科目类别名称已存在！';
                } else {
                    var name = $('input[name= SubjectTypeName]').val().toString();
                    //新增公司
                    $.ajax({
                        url: "/Financial/AddSubjectType",
                        data: { name: name },
                        type: "post",
                        dataType: "text",
                        success: function (data) {
                            if (data == "True") {
                                table2.reload({
                                    page: {
                                        curr: 1
                                    }
                                });
                                layer.msg('新增成功！'); 
                                layer.close(index2);
                            }
                            if (data == "False") {
                                
                                layer.msg('新增失败！');
                                layer.close(index2);
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
                        data: { Name: value.trim(), num: 1 },
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
                   var status2 = $('input[name="sexc"]:checked ').val().toString();
                   $.ajax({
                        url: "/Financial/UpSubjectType",
                        data: { name: value, status: status2, ID: id },
                        type: "post",
                        dataType: "text",
                        success: function (data) {
                            if (data == "True") {
                                layer.msg('修改成功！'); 
                                table2.reload({
                                    page: {
                                        curr: 1
                                    }
                                });   
                                layer.close(index3);
                            } else {
                                layer.msg('修改失败！');
                                layer.close(index3);
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
            id = data.编号, name = data.科目类别, status = data.状态;
            name2 = name;
            if (layEvent === 'detail') {
                layer.msg('查看操作');
            } else if (layEvent === 'del') {
                layer.confirm('真的删除行么？', function (index) {
                    $.ajax({
                        url: "/Financial/DlSubjectType",
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
                $('input[name= SubjectTypeName2]').val(name);
                if (status == '有效') {            
                    $("#rid1").prop("checked", true);
                    form.render();
                } else {
                    $("#rid2").prop("checked", true);
                    form.render();
                }
                index3 = layer.open({
                    type: 1
                    , title: '修改科目类别'
                    , area: ['50%', '40%']
                    , content: $("#for2")
                    , skin: ['layui-layer-molv']
                });             
            }
        });

    });
})