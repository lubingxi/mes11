$(function () {
    layui.use(['table', 'layer','form'], function () {
        var table = layui.table, layer = layui.layer,form=layui.form;          
        var tableIns=table.render({
                elem: '#tw'
                , url: '/SystemSettings/SeProductStation'
                , page: true
                , limit: 15
                , cols: [[
                      { field: '7', hide: true }
                    , { field: '序号', width: 50, title: '序号' }
                    , { field: '工位类型编号', width: 210, title: '工位类型编号', edit: 'text' }
                    , { field: '工位类型', width: 235, title: '工位类型', edit: 'text' }
                    , { width: 178, align: 'center', toolbar: '#barDemo'}
                ]]
            });       
        $("#Search").click(function () {
            var NameSearch = $("#Name").val().trim();
            tableIns.reload({
                where: {
                    Name: NameSearch                  
                }
            });
        });
   
      
        table.on('tool(demo)', function (obj) { //注：tool 是工具条事件名，test 是 table 原始容器的属性 lay-filter="对应的值"
            var data = obj.data //获得当前行数据
                , layEvent = obj.event; //获得 lay-event 对应的值                      
            if (layEvent === 'del') {
                var id = data.序号;
                layer.confirm('真的删除行么', function (index) {
                    $.ajax({
                        url: "/SystemSettings/DlProductStation",
                        data: { StationID: id },
                        type: "post",
                        dataType: "text",
                        success: function (data) {
                            if (data == "true") {
                                layer.msg('删除成功');
                            }
                        }
                    });
                    obj.del(); //删除对应行（tr）的DOM结构
                    layer.close(index);
                    //向服务端发送删除指令
                });
            } else if (layEvent === 'edit') {
                var id = data.序号;
                var Number = data.工位类型编号;
                var StationType = data.工位类型;
                $.ajax({
                    url: "/SystemSettings/UpProductStation",
                    data: { id:id,Number: Number, StationType: StationType, name: localStorage.sitename },
                    type: "post",
                    dataType: "text",
                    success: function (data) {
                        if (data == "true") {
                            layer.msg('修改成功');
                            $("#Search").click()
                        }
                    }
                });

            }
        });
    });
})