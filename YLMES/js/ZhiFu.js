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
        var note5 = '', km = '', ytu = '', mans = '', manf = '', manb = '', manl2 = '', money = '', note = '', date4 = '', dept2 = '';
        var index2 = Object;
        var index5 = Object;
        var index3 = Object, index4 = Object;
        var name2 = '', status = '', id = '';
        var flag = 0;
        var table2 = Object;
        var table3 = Object;
        //txr 点击事件
        $("#txt").click(function () {
            alert(55);
            $.ajax({
                url: "/Financial/datatest2", 
                type: "post",
                dataType: "text",
                success: function (data) {

                    layer.msg('98！');
                    //if (data == 'False2') {
                    //    alert(233);
                    //    $.ajax({
                    //        url: "/Financial/MergePdfFilesWithBookMark",
                    //        type: "post",
                    //        dataType: "text",
                    //        success: function (data) {

                    //            layer.msg('66！');

                    //        }
                    //    });
                    //}
              
                }
            });
   

           
        })
        table2 = table.render({
            elem: '#tw'
            , url: '/Financial/SeLaimingListzf'
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
                , { field: 'indexTpl', width: 80, title: '序号', fixed: 'left', templet: '#indexTpl', totalRowText: '合计：' }
                , { field: '单号', width: 180, title: '单号(查看)', event: 'xi', sort: true, templet: '#cc' }      
    
                , { field: '支付时间', width: 150, title: '支付时间' }
                , { field: '支付状态', width: 150, title: '支付状态' }
                , { field: '账号', width: 230, title: '账号' }
                , { field: '录入状态', width: 150, title: '录入状态' }
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

        //记账取消
        $("#qx5").on('click', function () {
            layer.close(index5);
        });
        //自定义验证规则--新增科目类别form表单验证
        form.verify({
            gss: function (value) {
                if (value.toString().trim() == "") {
                    return '必填不能为空！';
                } else {
                    dept2 = $('#gss5').val();
                    
                      //账号支付
                        $.ajax({
                            url: "/Financial/UpLaimingzhifu",
                            data: {
                                name: dept2, danh: danh
                            },
                            type: "post",
                            dataType: "text",
                            success: function (data) {
                                if (data == "True") {
                                    layer.msg('支付成功！');
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
                                    layer.msg('支付失败！');
                                }
                                layer.close(index5);
                            }
                        });
                    
                }
             
            }

        });
        var objData = {}
        table.on('tool(demo)', function (obj) { //注：tool 是工具条事件名，test 是 table 原始容器的属性 lay-filter="对应的值"
            var data = obj.data //获得当前行数据
                , layEvent = obj.event; //获得 lay-event 对应的值
            id = data.编号, ytu = data.用途, mans = data.审批, dept2 = data.报销部门,
                money = data.金额, date4 = data.时间, manl = data.领用人, manb = data.报销人, danh = data.单号,
                manf = data.报销部门主管复核, note = data.备注, ztai = data.状态, lrzt = data.录入状态;
            name2 = name;
            if (layEvent === 'detail') {
                //alert($('tr').index(obj.tr));//获取当前行数
                if (lrzt == '已记账') {
                    layer.msg('已记账不可支付！');
                } else {
                    select.render({
                        '#gss5': { data: "/Financial/SeCaption", value: "name", text: "name" }
                    });
                    index5 = layer.open({
                        type: 1
                        , title: '账号支付'
                        , area: ['45%', '30%']
                        , content: $("#for6")
                        , skin: ['layui-layer-molv']
                    });
                }



            } else if (layEvent === 'upload') {
                    indexs.forEach(function (value, index) {
                        delete files[value];
                })
               
                    $.ajax({
                        url: "/Financial/SeLaiminlj",
                        data: { id: danh, type: 'check2'  },
                        type: "post",
                        dataType: "text",
                        success: function (data) {
                            if (data != null) {
                                var dataObj = eval("(" + data + ")");//转换为json对象 
                                $.each(dataObj, function (name, value) {
                                    //alert(dataObj[name].col);
                                    $('#demo3').append(' <input name="like1" title="附件' + (++num) + ':" type="checkbox" value="' + dataObj[name].col + '" lay-skin="primary">'
                                        + '<img src="/Uploadf/'
                                        + dataObj[name].col + '" alt="' + dataObj[name].col + '" title="' + dataObj[name].col
                                        + '+" class="layui-upload-img imgt"  style="width:100px;height:100px;">')

                                });
                                form.render();
                            }
                        }
                    });
                    num = 0;
                    boo = false;
                    while (div.hasChildNodes()) {
                        div.removeChild(div.firstChild);
                    }
                    while (div2.hasChildNodes()) {
                        div2.removeChild(div2.firstChild);
                    }
                    index4 = layer.open({
                        type: 1
                        , title: '上传附件'
                        , cancel: function () {
                            // 你点击右上角 X 取消后要做什么
                            indexs.forEach(function (value, index) {
                                delete files[value];
                            })
                            num = 0;
                            boo = false;
                            while (div.hasChildNodes()) {
                                div.removeChild(div.firstChild);
                            }
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
 
            } else if (layEvent === 'fs') {
           
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


        //多图片上传
        var div = document.getElementById("demo2");
        var div2 = document.getElementById("demo3");
        var files, msg, code;
        var indexs = [];
        var boo = false;
        var a = '';
        var nus = 1;
        //附件删除
        $("#cs").on('click', function () {
            //获取复选框选中的值  
            var delstr = [];
            var objs = document.getElementsByName('like1');
            for (var i = 0; i < objs.length; i++) {
                if (objs[i].checked) {
                    delstr.push(objs[i].value);
                }
            }
            if (delstr.length > 0) {
                indexs.forEach(function (value, index) {
                    delete files[value];
                })
                num = 0;
                boo = false;
                while (div.hasChildNodes()) {
                    div.removeChild(div.firstChild);
                }
                while (div2.hasChildNodes()) {
                    div2.removeChild(div2.firstChild);
                }
                layer.close(index4);
                form.render();
                $.ajax({
                    url: "/Financial/DelLoadlj",
                    data: { delstr: delstr, id: danh,type:'del2' },
                    type: "post",
                    dataType: "text",
                    success: function (data) {
                        if (data == "True") {
                            layer.msg('删除成功！');
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
                        } else {
                            layer.msg('删除失败！');
                        }
                    }
                });
            } else {
                layer.msg("未选择附件！", { icon: 5, time: 800 })
            }
        });
        //附件上传取消
        $("#qx3").on('click', function () {
            indexs.forEach(function (value, index) {
                delete files[value];
            })
            num = 0;
            boo = false;
            while (div.hasChildNodes()) {
                div.removeChild(div.firstChild);
            }
            while (div2.hasChildNodes()) {
                div2.removeChild(div2.firstChild);
            }
            layer.close(index4);
        });
        //图片上传非空验证
        $("#bc3").click(function () {

            if (!boo) {
                layer.msg("未选择文件！", { icon: 5, time: 800 })
            }
            boo = false;
        })
        upload.render({
            elem: '#test2'
            , url: '/Financial/AddLaimingLoad'
            , data: {
                id: function () {
                    return $('#flag4').val();
                }
                , type:'up'
            }
            , accept: 'file'
            //, acceptMime: 'image/jpg, image/png'
            , auto: false //选择文件后不自动上传
            , bindAction: '#bc3' //指向一个按钮触发上传
            , multiple: true
            , choose: function (obj) {
                boo = true;
                //将每次选择的文件追加到文件队列
                files = obj.pushFile();
                $('#flag4').val(danh);

                //预读本地文件，如果是多文件，则会遍历。(不支持ie8/9)
                obj.preview(function (index, file, result) {
                    //console.log(index); //得到文件索引
                    //console.log(file); //得到文件对象
                    //console.log(result); //得到文件base64编码，比如图片

                    var today = new Date();
                    var s = today.getFullYear() + "" + today.getMonth() + "" + today.getDate() + "" + today.getHours() + "" + today.getMinutes() + "" + today.getSeconds();
                    a = s;
                    if (s == a) {
                        s = s + (++nus);
                    }
                    obj.resetFile(index, file, s + file.name); //重命名文件名，layui 2.3.0 开始新增
                    //这里还可以做一些 append 文件列表 DOM 的操作
                    $('#demo2').append('附件' + (++num) + ':<img src="' + result + '" alt="' + file.name + '" title="' + file.name + '+" class="layui-upload-img"  style="width:100px;height:100px;">')
                    //obj.upload(index, file); //对上传失败的单个文件重新上传，一般在某个事件中使用
                    indexs.push(index)
                });
            }
            , before: function (obj) {
                //预读本地文件示例，不支持ie8
                obj.preview(function (index, file, result) {
                    //$('#demo2').append('附件' + (++num) + ':<img src="' + result + '" alt="' + file.name + '" title="' + file.name + '+" class="layui-upload-img"  style="width:100px;height:100px;">')
                });
            }
            , done: function (res) { //每个文件提交一次触发一次。详见“请求成功的回调”
                //上传完毕
                //alert(res.msg);    
                msg = res.msg; code = res.code;
            }
            , allDone: function (obj) { //当文件全部被提交后，才触发
                //console.log(obj.total); //得到总文件数
                //console.log(obj.successful); //请求成功的文件数
                //console.log(obj.aborted); //请求失败的文件数                
                if (obj.total == obj.successful && code == 0) {
                    while (div.hasChildNodes()) {
                        div.removeChild(div.firstChild);
                    }
                    layer.msg('上传成功！');
                } else if (obj.total > obj.successful) {
                    layer.msg(obj.aborted + '个附件上传失败！');
                } else {
                    layer.msg('上传失败，' + msg);
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
                layer.close(index4);
                form.render();
            }
            , error: function (index, upload) {
                indexs.forEach(function (value, index) {
                    delete files[value];
                })
                layer.closeAll('loading'); //关闭loading
            }

        });
    });
})