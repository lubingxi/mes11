$(function () {
    var table, layer, form
    layui.use(['table','layer', 'form'], function () {
        table = layui.table, layer = layui.layer
        , form = layui.form,jquery=layui.$;
        $("#PurchaselistSel").click(function () {
            var PONO = $(".PONO").val().trim();
            var CreatedTime = $("#text1").val();
            var CreatedTimeEnd = $("#text2").val();          
            table.render({
                elem: '#tw'
                , url: '/ApplierList/GetPurchaselist?PONO=' + PONO + '&CreatedTime=' + CreatedTime + '&CreatedTimeEnd=' + CreatedTimeEnd 
                , page: true
                , limit: 15
                , cols: [[
                 { type: 'numbers', title: '序号', width: 60 }
               , { field: '图号', width: 90, title: '图号'}
               , { field: '物料名称', width: 180, title: '物料名称'}
               , { field: '物料规格', width: 180, title: '物料规格'}
               , { field: '物料材质', width: 70, title: '物料材质'}
               , { field: '采购数量', width: 90, title: '采购数量', edit: Text }
               , { field: '申请数量', width: 90, title: '申请数量' }
               , { field: '物料类别', width: 150, title: '物料类型', templet: '#Category' }
               , { field: '供应商', width: 150, title: '供应商', templet: '#gys'}
               , { field: '级别', width: 150, title: '级别', templet: '#Level'}
               , { field: '优势分析', width: 150, title: '优势分析', templet: 'Advantage'}
               , { field: '单价', width: 80, title: '单价', edit: Text}
               , { field: '总价', width: 80, title: '总价'}
               , { field: 'right', width: 178, align: 'center', toolbar: '#bsarDemo'}
                ]]
            });
            form.render();
           
        }); 
     
        var PurchaseID = null;
        var Category = null;
        var Level1 = null;
        var MaterialID = null;
        var Advantage1 = null;
        var ApplierProductTypeID = null;
        form.on('select(Category)', function (data) {
            var areaId = data.value;
            var oin = data.othis;
            ApplierProductTypeID = data.value;
           // layer.msg(ApplierProductTypeID);
            $.ajax({
                type: 'POST',
                url: '/ApplierList/Supp',
                data: { ids: areaId },
                dataType: 'json',
                success: function (data) {
                    var e = oin.parent().parent().next().children().children()
                    var r = oin.parent().parent().next().next().children().children();
                    var a = oin.parent().parent().next().next().next().children().children();
                    e.empty();
                    r.empty();
                    a.empty();
                    for (var i = 0; i < data.length; i++) {
                        e.append("<option>" + data[i].供应商 + "</option>");
                        r.append("<option>" + data[i].级别 + "</option>");
                        a.append("<option>" + data[i].优势分析 + "</option>");
                    }
                    if (data.length == 0) {
                        e.append("<dd>没有选项</dd>");
                        r.append("<dd>没有选项</dd>");
                        a.append("<dd>没有选项</dd>");
                    }
                    form.render();
                }
            });
          
        });
        form.on('select(Level)', function (data) {
            //得到select原始DOM对象
            Level1 = data.value;
           // layer.msg(Level1)
            //得到被选中的值

        });
        form.on('select(Advantage)', function (data) {
            //得到select原始DOM对象
            Advantage1 = data.value;
            //layer.msg(Advantage1)
            //得到被选中的值

        });
  
        table.on('tool(demo)', function (obj) { //注：tool 是工具条事件名，test 是 table 原始容器的属性 lay-filter="对应的值"
            var data = obj.data //获得当前行数据
                , layEvent = obj.event; //获得 lay-event 对应的值  
            PurchaseID = data.序号;
            var ApplyPurchaseQTY=data.采购数量;
            var UnitPrice = data.单价;
            Category = data.物料名称;
            MaterialID = data.物料材质;
         if (layEvent === 'del') {
                layer.confirm('真的删除行么', function (index) {
                    $.ajax({
                        url: "/ApplierList/ApplierListdel",
                        data: { PurchaseID: PurchaseID },
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
         } else if (layEvent === 'updata') {
            
                $.ajax({
                    type: "post",
                    url: "/ApplierList/UnitPricetUpdata",
                    data: { PurchaseID: PurchaseID, ApplyPurchaseQTY: ApplyPurchaseQTY, UnitPrice: UnitPrice,MaterialID:MaterialID,ApplierProductTypeID:ApplierProductTypeID },
                    dataType: "text",
                    success: function (data) {
                        if (data == "true") {
                            layer.msg('修改成功');
                            var Category1 = null;
                            var Level1 = null;
                            var Advantage1 = null;
                            $("#PurchaselistSel").click()
                        } else if (data =="false") {
                            layer.msg('修改失败');
                        }
                    }
                });
         } else if (layEvent === 'bangdi') {
          
         }
        });
     
    });

});