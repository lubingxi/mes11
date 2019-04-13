$(function () {
    layui.config({
        base: '/js/'
    }
    ).use(['upload', 'laydate', 'table', 'layer', 'form','Select'], function () {
        var table = layui.table, layer = layui.layer, form = layui.form, laydate = layui.laydate, upload = layui.upload;
        var dates = $("#dates").val();
        var datee = $("#datee").val();
        var num = 0;
        var select = layui.Select
        var ytu = '', mans = '', manf = '', manb = '', manl2 = '', money = '', note = '', date4 = '', dept2 = '', gssd='',danh='',cg='';
        var index2 = Object;
        var index3 = Object;
        var index3 = Object, index4 = Object, index5 = Object;
        var name2 = '', status = '', id = '';
        var flag = 0;
        var table2 = Object;
        var table3 = Object;
        var sexgc = '';
        table2 = table.render({
            elem: '#tw'
            , url: '/Financial/SeLaimingList?id=' + $('#Sessioname').val() 
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
                , { field: '费用部门', width: 150, title: '费用部门' }
                , { field: '单号', width: 170, title: '单号(查看)', event: 'xi', sort: true, templet: '#cc' }      
                , { field: '用途', width: 150, title: '用途', style: "color: #009688" }
                , { field: '金额', width: 150, title: '金额' }
                , { field: '时间', width: 150, title: '时间' }
                , { field: '附件数量', width: 150, title: '附件数量' }
                , { field: '领用人', width: 150, title: '领用人' }
                , { field: '报销人', width: 150, title: '报销人' }
                , { field: '报销部门主管复核', width: 150, title: '报销部门主管复核' }
                //, { field: '审批', width: 150, title: '审批' }
                , { field: '状态', width: 150, title: '状态' }
                , { field: '备注', width: 150, title: '备注' }
                , { field: 'right', width: 270, align: 'center', title: '操作', toolbar: '#barDemo' }
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
        $('#demo4').on('click', '.imgt', function () {
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
                    , datee: datee ,
                    id: $('#Sessioname').val(),
                    danh: $('#din').val(),
                     status: $('#gssz').val() 
                }
                , page: {
                    curr: 1
                }
            });
            form.render();
        });
        //新增
        $('#add').on('click', function () {
 
            flag = 1;
            select.render({
                '#gss': { data: "/Financial/SeDepts", value: "编号", text: "部门" }
            }, function (data) {
                layui.each(data["#gss"].data, function (index, value) {
                    objData[value.编号] = value;
                    //if (value.部门 == '行政人事') {
                    //    alert(value.部门);
                    //    cg = value.经理;
                    //}
                })
            });
            index2 = layer.open({
                type: 1
                , title: '新增报销单'
                , area: ['50%', '93%']
                , content: $("#for")
                , skin: ['layui-layer-molv']
            });
        });
        //报销单续增验证
        form.on('radio(b)', function (data) {
            
            sexgc = $('input[name="sexb"]:checked ').val().toString().trim();
            if (sexgc == '是') {
               
                $("#gssd").attr("disabled", false); 
             
                //加载单号下拉框
               
                select.render({
                    "#gssd": {
                        data: "/Financial/SeDanhList?yh=" + $('#Sessioname').val(), value: "name", text: "name", duf: 1
                    }
                });
                form.render();
            } else if (sexgc == '否') {
               
                $("#gssd").attr("disabled", true); 
                $("#gssd").empty();
                
                form.render();
            }
        });
        //新增取消
        $("#qx").on('click', function () {
            layer.close(index2);
        });
        //修改取消
        $("#q2x").on('click', function () {
            layer.close(index3);
        });
 
        //自定义验证规则--新增科目类别form表单验证
        form.verify({
            gssd: function (value) {
                if (sexgc == '是') {
                    if (value.toString().trim() == "") {
                        return '必填不能为空！';
                    }
                }
             
                gssd = $('#gssd').val();
                if (gssd == null || gssd == '') {
                    var a = '';
                    var today = new Date();
                    var s = today.getFullYear() + "" + today.getMonth() + "" + today.getDate() + "" + today.getHours() + "" + today.getMinutes() + "" + today.getSeconds();
                    //a = s;
                    //if (s == a) {
                    //    s = s + (++nus);
                    //}
                    gssd ='BXD'+ s;
                }
            }
            ,gss: function (value) {
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
                        //新增报销单
                        $.ajax({
                            url: "/Financial/AddLaiming",
                            data: {
                                ytu: ytu, mans: mans, manf: manf,
                                manb: manb, manl: manl, money: money,
                                note: note, date4: date4, dept: dept2, id: $('#Sessioname').val()
                                , danh: gssd
                            },
                            type: "post",
                            dataType: "text",
                            success: function (data) {
                                if (data == "True") {
                                    layer.msg('新增成功！');
                                    table2.reload({
                                        where: {
                                            dates: dates
                                            , datee: datee,
                                            id: $('#Sessioname').val(),
                                            //id: sesionnameid,
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
                    } else if (flag==2) {
                        //修改报销单
                        $.ajax({
                            url: "/Financial/UpLaiming",
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
                                            dates: dates
                                            , datee: datee,
                                            id: $('#Sessioname').val(),
                                            //id: sesionnameid,
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
                money = data.金额, date4 = data.时间, manl = data.领用人, manb = data.报销人,
                manf = data.报销部门主管复核, note = data.备注,ztai=data.状态,danh=data.单号;
            name2 = name;
            if (layEvent === 'detail') {
                if (ztai == '新建') {
                    layer.confirm('真的提交么？', function (index) {
                        $.ajax({
                            url: "/Financial/UpLaimingztai",
                            data: { id: id },
                            type: "post",
                            dataType: "text",
                            success: function (data) {
                                if (data == "True") {
                                    layer.msg('提交成功！');
                                } else {
                                    layer.msg('提交失败！');
                                }
                                table2.reload({
                                    where: {
                                        dates: dates
                                        , datee: datee,
                                        id: $('#Sessioname').val(),
                                        danh: $('#din').val(),
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
                    layer.msg('已提交！');
                }
            } else if (layEvent === 'upload') {
                //if (ztai == '已审核') {
                //    layer.msg('已审核不可上传！');
                //} else {
                indexs.forEach(function (value, index) {
                    delete files[value];
                })
                $.ajax({
                    url: "/Financial/SeLaiminlj",
                    data: { id: id, type:'check'},
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
                //}
            } else if (layEvent === 'del') {
                if (ztai == '已审核') {
                    layer.msg('已审核不可删除！');
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
                if (ztai == '已审核') {
                    layer.msg('已审核不可修改！');
                } else {
                    flag = 2;
                    select.render({
                        '#gss2': { data: "/Financial/SeDepts", value: "编号", text: "部门", duf: dept2 }
                    }, function (data) {
                        layui.each(data["#gss2"].data, function (index, value) {
                            objData[value.编号] = value;
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
                    , url: '/Financial/SeLaimingListDanh?id=' + $('#Sessioname').val() + '&danh=' + danh
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

            } else if (layEvent === 'uploadk') {
                var num = 0;
                $.ajax({
                    url: "/Financial/SeLaiminlj",
                    data: { id: id, type: 'check' },
                    type: "post",
                    dataType: "text",
                    success: function (data) {
                        if (data != null) {
                            var dataObj = eval("(" + data + ")");//转换为json对象 
                            $.each(dataObj, function (name, value) {
                                //alert(dataObj[name].col);
                                $('#demo4').append(' 附件' + (++num) + ':'
                                    + '<img src="/Uploadf/'
                                    + dataObj[name].col + '" alt="' + dataObj[name].col + '" title="' + dataObj[name].col
                                    + '+" class="layui-upload-img imgt"  style="width:100px;height:100px;">')

                            });
                            form.render();
                        }
                    }
                });

                index5 = layer.open({
                    type: 1
                    , title: '上传附件'
                    , cancel: function () {
                        // 你点击右上角 X 取消后要做什么

                        while (div3.hasChildNodes()) {
                            div3.removeChild(div3.firstChild);
                        }
                    }
                    , area: ['80%', '75%']
                    , content: $("#for4")
                    , skin: ['layui-layer-molv']
                });
            }
        });
        form.on('select(gss2)', function (data) {
            $('input[name= mans2]').val(''); 
            $('input[name= manf2]').val('');      
            //if (objData[data.value].部门 == '采购部') {
            //    $('input[name= mans2]').val(cg);   
            //}
            $('input[name= manf2]').val(objData[data.value].经理);      
        })
        form.on('select(gss)', function (data) {
            $('input[name= mans]').val('');
            $('input[name= manf]').val('');
            //if (objData[data.value].部门 == '采购部') {
            //    $('input[name= mans]').val(cg);
            //}
            $('input[name= manf]').val(objData[data.value].经理);
        })
   
        //多图片上传
        var div = document.getElementById("demo2");
        var div2 = document.getElementById("demo3");
        var div3 = document.getElementById("demo4");
        var files,msg,code; 
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
                    data: { delstr: delstr, id: id,type:'del' },
                    type: "post",
                    dataType: "text",
                    success: function (data) {
                        if (data == "True") {
                            layer.msg('删除成功！');
                            table2.reload({
                                where: {
                                    dates: dates
                                    , datee: datee,
                                    id: $('#Sessioname').val(),
                                    danh: $('#din').val(),
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
                layer.msg("未选择文件！", {icon:5,time:800})
            }
            boo = false;
        })
        upload.render({
            elem: '#test2'
            , url: '/Financial/AddLaimingLoad'
            , data: {
                 id: function() {
                    return $('#flag4').val();
                }
                ,type:'up2'
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
                $('#flag4').val(id);
               
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
                    $('#demo2').append('附件' + (++num) + ':<img src="' + result+ '" alt="' + file.name + '" title="' + file.name + '+" class="layui-upload-img"  style="width:100px;height:100px;">')
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
                msg = res.msg;code= res.code;
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
                    layer.msg('上传失败，'+msg);
                }
                table2.reload({
                    where: {
                        dates: dates
                        , datee: datee,
                        id: $('#Sessioname').val(),
                        danh: $('#din').val(),
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