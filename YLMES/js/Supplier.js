$(function () {
    layui.use(['table', 'layer', 'form'], function () {
        var table = layui.table, layer = layui.layer
        , form = layui.form;
        var tableIns=table.render({
                elem: '#tw'
                , url: '/ApplierList/GetSupplierlist'
                , page: true
                , height: 'full-20'
                , limit: 15
                , cols: [[
                  { field: '', hide:true }
                ,{ field: '序号', width: 80, title: '序号' }
               , { field: '名称', width: 205, title: '名称', edit: Text,style:"color:#009688" }
                    , { field: '地址', title: '地址', width: 210, edit: Text, event: 'setSign', style: "color:#009688" }
                    , { field: '联系人', width: 90, title: '联系人', edit: Text, style: "color:#009688" }
                    , { field: '电话', width: 120, title: '电话', edit: Text, style: "color:#009688" }
                    , { field: '手机', width: 120, title: '手机', edit: Text, style: "color:#009688" }
                    , { field: '级别', width: 120, title: '级别', toolbar: "#Level" }
                    , { field: '优势分析', width: 200, title: '优势分析  ', toolbar: "#Advantage" }
               , { field: '审核状态', width: 100, title: '审核状态' }
               , { field: 'right', width: 300, align: 'center', toolbar: '#bsarDemo' }
                ]]
            });      
        $("#SupplierSel").click(function () {
            var NameSearch = $("#Name").val().trim();
            var Select = $("#s option:selected").text();
            tableIns.reload({
                page: {
                    curr: 1
                },
                where: {
                    Name: NameSearch
                    , Status: Select
                }
            });
        })
  
        var Level1 = null;
        var Advantage1 = null;
        form.on('select(Level)', function (data) {
            //得到select原始DOM对象
            Level1 = data.value;            
            //得到被选中的值
        });
        form.on('select(Advantage)', function (data) {
            //得到select原始DOM对象
            Advantage1 = data.value;      
            //得到被选中的值

        });
       


        table.on('tool(demo)', function (obj) { //注：tool 是工具条事件名，test 是 table 原始容器的属性 lay-filter="对应的值"
            var data = obj.data //获得当前行数据
            , layEvent = obj.event; //获得 lay-event 对应的值  
            var ApplierID = data.序号;
            var ApplierName = data.名称;
            var Address = data.地址;
            var Contact = data.联系人;
            var Tel = data.电话;
            var Mobile = data.手机;
            var Level = Level1;
            var Advantage = Advantage1;
            if (obj.event === 'setSign') {
                layer.prompt({
                    formType: 2
                    , title: '修改地址'
                    , value: data.sign
                }, function (value, index) {
                    layer.close(index);
                    $.ajax({
                        url: "/ApplierList/EditDress",
                        data: { ApplierID: ApplierID, Dress: value},
                        type: "post",
                        dataType: "text",
                        success: function (data) {
                            if (data == "true") {
                                layer.msg('修改成功');
                                $("#SupplierSel").click();
                            }
                        }
                    });
                });
            }
            if (Level1 == null) {
                Level = data.级别;
            }
            if (Advantage1 == null) {
                Advantage = data.优势分析;
            }
            if (layEvent === 'detail') {
                
            } else if (layEvent === 'del') {
                layer.confirm('真的删除行么', function (index) {
                    $.ajax({
                        url: "/ApplierList/ApplierListdel",
                        data: { ApplierID: ApplierID },
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
            }
            else if (layEvent === 'add') {
                window.parent.document.getElementById("rightframe").src = '/ApplierList/AddMaterInfo?SupplierId=' + ApplierID;
            }
            else if (layEvent === 'check') {
                window.parent.document.getElementById("rightframe").src = '/ApplierList/CheckMaterInfo?SupplierId=' + ApplierID
            }
            else if (layEvent === 'edit') {
                $.ajax({
                    type: "post",
                    url: "/ApplierList/ApplierListUpdata",
                    data: { ApplierID: ApplierID, ApplierName: ApplierName, Address: Address, Contact: Contact, Tel: Tel, Mobile: Mobile,Level: Level, Advantage: Advantage },
                    dataType: "text",
                    success: function (data) {
                        if (data == "true") {
                            layer.msg('修改成功');
                            var Category1 = null;
                            var Level1 = null;
                            var Advantage1 = null;
                            $("#SupplierSel").click()
                        }
                    }
                });

            } else if (layEvent === 'sh' && data.审核状态 === "未审核") {
                    layer.confirm('是否通过审核！', {
                        title: "友力提示", skin: "my-skin",
                        btn: ['确定', '取消'], btn1: function () {
                            layer.closeAll();
                            $.ajax({
                                url: "/ApplierList/ApplierListReview?id="+data.序号,
                                type: 'post',
                                success: function (data) {
                                    if (data == "true") {
                                        layer.msg("修改成功", { icon: 6 });
                                        $("#SupplierSel").click();
                                    }
                                    if (data == "false") {
                                        layer.msg('修改失败!', { icon: 5 });
                                        return false;
                                    }
                                }
                            });
                        }
                    });
            } else if (layEvent === 'bb')
            {
               layer.open({
                    type: 2
                 , content: '/ApplierList/Material?ApplierID=' + ApplierID
                  , area: ['80%', '80%']
                 , anim: 2
                });
            }
          
        });
    });
});