!function ($) {
    /* 基于Bootstrap Typeahead改造而来的自动完成插件
     * Author：F.L.F
     * Site: http://digdata.me
     * ================================= */


    //扩展
    $.fn.extend(
    {
        //默认调用方式$('#ObjID').autocompletebind("?Action=GetDB");
        //服务端返回格式(默认)：[{"value":2,"text","abc"},{"value":3,"text","abd"}]这样的数据
        //回调参数:callbackfun:function(d){d.text,d.value}
        autocompletebind: function (Url, options) {
            //Url参数也可以是一个function(){..}有时候url可能会根据需求变动
            var options = $.extend({}, options)
            //var DataSourcesJson = options.datasources || null;//这个功能还没有写
            var ShowValue = options.showvalue || 'item["text"]';//输入显示值，如 Name
            var DataValue = options.datavalue || ShowValue;//选中后input中的值//可以不设置
            var RealValue = options.realvalue || 'item["value"]';//选中后real-vluae属性的值，如 ID
            var parameteName=options.parameteName || 'AjaxQuery';//ajax请求的参数名称，默认名称都为AjaxQuer
            var postDataStyle=options.postDataStyle || 1;//1默认，2外面再套一层叫{json:{ajaxeurye:'aaa'}}  /内部ajax外部的数据
            var callbackfun = options.callbackfun;//选中后的回调函数
            var isUpperCase=options.isUpperCase==true?true:false;//输入字符转大写(不用必填)zw2020-4-27
            var isLowerCase=options.isLowerCase==true?true:false;//输入字符转小写(不用必填)zw2019-4-27
            var currentItem = options.model || ''//数组当前操作项
            var headers_ = options.headers;
            
            var urlParams=options.urlParams;
            // console.log("BBBBBBBBBBBBBBBBBBBBB");
            // if(options.urlParams)console.log(urlParams[0]);

           $(this).autocomplete({
                freelyInput:options.freelyInput==true?true:false,//是否可以自由输入zw2019-3-18
                focused:options.focused==true?true:false,//是否从focused的时候加载的插件zw2019-3-18
                isUpperCase:isUpperCase,
                isLowerCase:isLowerCase,
                source: function (query, process) {
                    var matchCount = this.options.items;
                    var _url;//因为有动态地址所以这里要这样写
                    // console.log("----------------------------")
                    // console.log(Url)
                    // console.log(typeof (Url))
                    if (typeof (Url) == "function") {
                        //_url = Url(params[0],params[1]);
                        _url=Url.apply(this, urlParams);
                    } else {
                        _url = Url;
                    }
                    
                    // console.log("DDDDDDDDDDDDDDDDDDDDDDDDDDDD");
                    var postData={};
                    if (postDataStyle == 2) {//外面套了一层data的格式
                        var json = { data: {} };
                        json.data[parameteName] = query;
                        postData = {
                            json: JSON.stringify(json)
                        };
                    } else if (postDataStyle == 3) {
                        postData[parameteName] = query;
                    } else { 
                        _url += "?AjaxQuery=" + query;
                    }

                    $.ajax({ url: _url,
                        type: "POST",
                        data: JSON.stringify(postData),
                        crossDomain: true,//加这二行支持ajax跨域
                        xhrFields: {withCredentials: true},//加这二行支持ajax跨域
                        headers: headers_,
                        success: function (respData) {
                            respData = respData.result;
                            //if(postDataStyle==2 && respData.success){
                            //    respData = respData.result;
                            //}
                            if (respData == 'null' || respData == '') {
                                //$(ThisObj).removeAttr("real-value").removeAttr("title");
                                return;
                            }
                            if (typeof (respData) == "object") {
                                //如果直接是一个json不用格式化
                                return process(respData);
                            } else {
                                try {
                                    return process($.parseJSON(respData));
                                } catch (error) {
                                    return null;
                                }
                            }
                    }});


                    /**
                        $.post(_url, { "AjaxQuery": query }, function (respData) {
                        if (respData == 'null' || respData == '') {
                            //$(ThisObj).removeAttr("real-value").removeAttr("title");
                            return;
                        }
                        if (typeof (respData) == "object") {
                            //如果直接是一个json不用格式化
                            return process(respData);
                        } else {
                            try {
                                return process($.parseJSON(respData));
                            } catch (error) {
                                return null;
                            }
                        }
                    });
                     **/
                },
                formatItem: function (item) {
                    //$(ThisObj).removeAttr("real-value").removeAttr("title");
                    return eval(ShowValue);
                },
                setValue: function (item) {
                    //console.log("111:"+eval(DataValue) + "--" + eval(RealValue)+"---"+JSON.stringify(item));
                    var DataValueV=eval(DataValue);
                    if(isUpperCase){DataValueV=DataValueV.toUpperCase();}
                    if(isLowerCase){DataValueV=DataValueV.toLowerCase();}
                    //返回原始数据Smart2017-6-7
                    return { 'data-value': DataValueV, 'real-value': eval(RealValue),'json':JSON.stringify(item) };
                },
                callback: function (item) {
                    //console.log("oldcallback:" + item.text + "--" + item.value);
                    if (callbackfun != undefined && typeof (callbackfun) == "function") callbackfun(item,currentItem);
                    return false;
                }

            });
        }
        //以下方法会生成一个新的input，原来的input不变，value存的real-value的值
        //默认调用方式$('#ObjID').autocompleteNew("?Action=GetDB",[默认值可省]{text: "abc",value: "123"});
        //可调多个,请求地址可变，带默认值，带回调,$('.input').autocompleteNew(function(){.....return url;},{text: "abc",value: "123",callbackfun:function(item){alert(d.value+"--"+d.text);}});
        //服务端返回格式(默认)：[{"value":2,"text","abc"},{"value":3,"text","abd"}]这样的数据
        //回调参数:callbackfun:function(d){d.text,d.value} d为回参数
        , autocompleteNew: function (Url, options) {
            var options = $.extend({}, options);
            var tempfun = options.callbackfun;
            $(this).each(function () {
                var SourceInput = $(this);
                var DisplayInput = $(SourceInput).clone();//clone一个出来这样直接取值了
                $(DisplayInput).attr("autocomplete", "off");//自带记忆输入提示禁用
                var freelyInput=options.freelyInput==true?true:false;
                var name = $(SourceInput).attr("name");
                var id = $(SourceInput).attr("id");
                if (name != undefined) $(DisplayInput).attr("name", name + "_Display");
                if (id != undefined) $(DisplayInput).attr("id", id + "_Display");
                $(SourceInput).before(DisplayInput).hide();
                $(DisplayInput).on("change", function () {$(this).val() == "" && $(SourceInput).val("");});
                var Value = options.value || '';//默认值value
                var RealValue = options.realvalue || '';//默认值value
                var Text = options.text || '';//默认显示内容Text

                if (Value != "") { $(SourceInput).val(Value); $(DisplayInput).attr("real-value", Value); }
                if (RealValue != "") { $(SourceInput).val(RealValue); $(DisplayInput).attr("real-value", RealValue); }
                if (Text != "") $(DisplayInput).val(Text);

                options.callbackfun = function (item) {
                    //console.log(item);
                    if(freelyInput){
                        $(SourceInput).val($(DisplayInput).val());
                        item=item || {text:$(DisplayInput).val()}
                        if (tempfun != undefined && typeof (tempfun) == "function") tempfun(item);
                    }else {
                        //item为反回的text和value josn格式，如果没有选中返回undefined
                        if (item != undefined && item != null) {
                            //onsole.log("newback:" + item.text + "--" + item.value);
                            var v = $(DisplayInput).attr("real-value") || "";
                            //console.log("v:" + v);
                            $(SourceInput).val(v);//回调的时候赋值到原来的控件上面
                            if (tempfun != undefined && typeof (tempfun) == "function") tempfun(item);
                        }
                        else {
                            //console.log("啥也没选！删除操作")
                            if (tempfun != undefined && typeof (tempfun) == "function") tempfun(item);
                        }
                    }
                }
                $(DisplayInput).autocompletebind(Url, options);//调用老的
            });
        }
        //输入提示多个：只有默认参数不一样，其他都一样
        //$("#CompanyID").autocompleteMore("/Common/GetCompanyList", {data: [{ "text": "aaaa", "value": 1 }, { "text": "bbb", "value": 2 }, { "text": "cccc", "value": 3 }]}); callbackfun()选中后事件delcallbackfun()删除事件
        , autocompleteMore: function (Url, options) {
            var options = $.extend({}, options);
            var tempfun = options.callbackfun;

            $(this).each(function () {
                var SourceInput = $(this);
                var DisplayInput = document.createElement("input"); //创建一个
                var placeholder=options.placeholder || "";
                options.isRepeat=options.isRepeat || false;//默认不能重复选择
                //var DisplayInput = $(SourceInput).clone();

                $(DisplayInput).attr("type", "text").attr("autocomplete", "off").attr("placeholder",placeholder);//自带记忆输入提示禁用
                var name = $(SourceInput).attr("name");
                var id = $(SourceInput).attr("id");
                if (name != undefined) $(DisplayInput).attr("name", name + "_Display");
                if (id != undefined) $(DisplayInput).attr("id", id + "_Display");

                var style = options.style || "";
                var liClass=options.liFixWidth ? "diming":"";//<li>是否固定宽度
                var liStyle = options.liStyle || "";//<li>的style
                var NewUL = document.createElement("div");
                $(NewUL).click(function(){$(DisplayInput).focus();});//点击自动焦点.
                $(NewUL).addClass("dropdown_butul").append(DisplayInput).attr("style", style);
                $(SourceInput).before(NewUL).hide();
                //样式

                //默认值
                var defaultData = options.data || [];//默认值value
                var defaultValue = "";
                for (var i = 0; i < defaultData.length; i++) {
                    defaultValue += defaultValue == "" ? defaultData[i].value : "," + defaultData[i].value;
                    var newLI = $("<li class='"+liClass+"' style='"+liStyle+"' title='" + defaultData[i].text + "'>" + defaultData[i].text + "<a title='Click to delete this' real-value='" + defaultData[i].value + "'>x</a></li>");
                    $(newLI).find("a").click(function () {

                        var realvalue = $(this).attr("real-value");
                        var OldValue = $(SourceInput).val();
                        var vArr = OldValue.split(",");
                        var vNew = "";
                        for (var i = 0; i < vArr.length; i++) { if (vArr[i] != "" && vArr[i] != realvalue) vNew += (vNew == "" ? vArr[i] : "," + vArr[i]); }
                        //console.log("OldValue:"+vNew+"==OldValue:"+vNew)
                        $(SourceInput).val(vNew);

                        $(this).empty()//这个不能删，否则 $(this).parent().text()获取到的值里面会默认戴 一个 "x"
                        var it={value:realvalue,text:$(this).parent().text()}
                        $(this).parent().remove();
                        if (options.delcallbackfun != undefined && typeof (options.delcallbackfun) == "function")
                        {
                            options.delcallbackfun(it);
                        }
                    });
                    $(DisplayInput).before(newLI);
                }
                $(SourceInput).val(defaultValue);
                //是否显示SelectAll按钮,如果选中将value设置为0
                var selectAll = options.selectall || false;
                if (selectAll) {
                    var selectAllBut = $("<input type='button' value='Selcted All' />");
                    $(selectAllBut).click(function () {
                        if ($(this).val() == "Selcted All") {
                            $(this).val("Not Selcted All");
                            $(SourceInput).val("0");
                            $(NewUL).children("input:text").hide();
                            $(NewUL).children("li").hide();
                            var newLI = $("<li class='"+liClass+"' style='"+liStyle+"' name='All'>All<a>x</a></li>");
                            $(newLI).find("a").click(function () { $(selectAllBut).trigger("click"); });
                            $(DisplayInput).before(newLI);

                        } else {
                            $(this).val("Selcted All");
                            var v = $(NewUL).children("li").map(function () { return $(this).find("a").attr("real-value"); }).get().join(", ");
                            $(SourceInput).val(v);
                            $(NewUL).children("input:text").show();
                            $(NewUL).children("li").show();

                            $(NewUL).find("li[name='All']").remove();
                        }
                        //console.log($(SourceInput).val());
                    });
                    $(DisplayInput).after(selectAllBut);
                }
                //回调函数
                options.callbackfun = function (item) {
                    //item为反回的text和value josn格式，如果没有选中返回undefined
                    if (item != undefined && item != null) {
                        //console.log(item);

                        //console.log($(SourceInput).val());
                        var SourceInputValue = $(SourceInput).val();
                        //console.log($(SourceInput).val());
                        //判断重复
                        var Temp=","+SourceInputValue+",";
                        if(!options.isRepeat && Temp.indexOf(","+item.value+",")>-1){
                            //已存在
                            if(layer){
                                layer.msg("Can't repeat");
                            }else{
                                alert("Can't repeat")
                            }
                            $(DisplayInput).val("").attr("real-value", "");//清空输入框的值
                        }else {

                            var isAppend=true;//是否添加
                            if (tempfun != undefined && typeof (tempfun) == "function") isAppend=tempfun(item);//callbackfun返回false时不添加新项
                            //console.log("isAppend:"+isAppend)
                            if(isAppend==undefined || isAppend==true) {
                                if (SourceInputValue == "") {
                                    SourceInputValue = item.value;
                                } else {
                                    SourceInputValue = SourceInputValue + "," + item.value;
                                }
                                $(SourceInput).val(SourceInputValue);//回调的时候赋值到原来的控件上面

                                //jquery方法
                                var newLI = $("<li class='" + liClass + "' style='" + liStyle + "' title='" + item.text + "'>" + item.text + "<a real-value='" + item.value + "'>x</a></li>");
                                $(newLI).find("a").click(function () {
                                    var realvalue = $(this).attr("real-value");
                                    var OldValue = $(SourceInput).val();
                                    var vArr = OldValue.split(",");
                                    var vNew = "";
                                    for (var i = 0; i < vArr.length; i++) {
                                        if (vArr[i] != "" && vArr[i] != realvalue) vNew += (vNew == "" ? vArr[i] : "," + vArr[i]);
                                    }
                                    $(SourceInput).val(vNew);
                                    $(this).empty()//这个不能删，否则 $(this).parent().text()获取到的值里面会默认戴 一个 "x"
                                    var it = {value: realvalue, text: $(this).parent().text()}
                                    $(this).parent().remove();
                                    if (options.delcallbackfun != undefined && typeof (options.delcallbackfun) == "function") {
                                        options.delcallbackfun(it);
                                    }

                                });
                                $(DisplayInput).before(newLI);
                                //console.log("--SourceInputValue:" + $(SourceInput).val());

                            }
                            $(DisplayInput).val("").attr("real-value", "");//清空输入框的值


                        }

                    }

                }
                $(DisplayInput).autocompletebind(Url, options);//调用老的
            });
        }

        //带有全选功能
        , autocompleteAll: function (Url, options) {
            var options = $.extend({}, options);
            var tempfun = options.callbackfun;
            var myInput = $(this);
            myInput.after("<input type='button' value='All' id='SelectOptionAll'>");
            $(this).each(function () {
                var SourceInput = $(this);
                var DisplayInput = document.createElement("input"); //创建一个
                $(DisplayInput).attr("type", "text").attr("autocomplete", "off");//自带记忆输入提示禁用
                var name = $(SourceInput).attr("name");
                var id = $(SourceInput).attr("id");
                if (name != undefined) $(DisplayInput).attr("name", name + "_Display");
                if (id != undefined) $(DisplayInput).attr("id", id + "_Display");

                var style = options.style || "";
                var NewUL = document.createElement("div");
                $(NewUL).addClass("dropdown_butul").append(DisplayInput).attr("style", style);
                $(SourceInput).before(NewUL).hide();
                //样式

                //默认值
                var defaultData = options.data || [];//默认值value
                var defaultValue = "";

                for (var i = 0; i < defaultData.length; i++) {
                    defaultValue += defaultValue == "" ? defaultData[i].value : "," + defaultData[i].value;
                    var newLI = $("<li title='" + defaultData[i].text + "'>" + defaultData[i].text + "<a real-value='" + defaultData[i].value + "'>x</a></li>");
                    $(newLI).find("a").click(function () {
                        var realvalue = $(this).attr("real-value");
                        var OldValue = $(SourceInput).val();
                        var vArr = OldValue.split(",");
                        var vNew = "";
                        for (var i = 0; i < vArr.length; i++) { if (vArr[i] != "" && vArr[i] != realvalue) vNew += (vNew == "" ? vArr[i] : "," + vArr[i]); }
                        $(SourceInput).val(vNew);
                        $(this).parent().remove();
                    });
                    $(DisplayInput).before(newLI);
                }

                //动态添加All按钮
                $(this).next('#SelectOptionAll').click(function () {
                    $(DisplayInput).siblings('li').remove();
                    $(DisplayInput).val('All');
                    $(DisplayInput).attr('real-value', '0');
                    $(SourceInput).val('0');
                })
                $(SourceInput).val(defaultValue);
                //回调函数
                options.callbackfun = function (item) {
                    //item为反回的text和value josn格式，如果没有选中返回undefined
                    if (item != undefined && item != null) {
                        //console.log("newback:" + item.text + "--" + item.value);

                        var SourceInputValue = $(SourceInput).val();
                        if (SourceInputValue == "") {
                            SourceInputValue = item.value;
                        } else {
                            SourceInputValue = SourceInputValue + "," + item.value;
                        }
                        $(SourceInput).val(SourceInputValue);//回调的时候赋值到原来的控件上面
                        //jquery方法
                        var newLI = $("<li title='" + item.text + "'>" + item.text + "<a real-value='" + item.value + "'>x</a></li>");
                        $(newLI).find("a").click(function () {
                            var realvalue = $(this).attr("real-value");
                            var OldValue = $(SourceInput).val();
                            var vArr = OldValue.split(",");
                            var vNew = "";
                            for (var i = 0; i < vArr.length; i++) { if (vArr[i] != "" && vArr[i] != realvalue) vNew += (vNew == "" ? vArr[i] : "," + vArr[i]); }
                            $(SourceInput).val(vNew);
                            $(this).parent().remove();
                        });
                        $(DisplayInput).before(newLI);
                        $(DisplayInput).val("").attr("real-value", "");//清空输入框的值
                        //console.log("--SourceInputValue:" + $(SourceInput).val());
                        if (tempfun != undefined && typeof (tempfun) == "function") tempfun(item);
                    }

                }
                $(DisplayInput).autocompletebind(Url, options);//调用老的
            });
        }
    });


    var Autocomplete = function (element, options) {
        this.$element = $(element)
        $(element).attr("autocomplete", "off");//自带记忆输入提示禁用
        $(element).attr("bindautocomplete", "true");//自带记忆输入提示禁用

        this.options = $.extend({}, $.fn.autocomplete.defaults, options)
        this.sorter = this.options.sorter || this.sorter
        this.highlighter = this.options.highlighter || this.highlighter
        this.updater = this.options.updater || this.updater
        this.source = this.options.source
        this.$menu = $(this.options.menu)
        this.shown = false
        this.formatItem = this.options.formatItem || this.formatItem
        this.setValue = this.options.setValue || this.setValue
        this.freelyInput=this.options.freelyInput==true?true:false;//自由输入(不用必填)zw2019-3-18
        this.options.isUpperCase=this.options.isUpperCase==true?true:false;//输入字符转大写(不用必填)zw2020-4-27
        this.options.isLowerCase=this.options.isLowerCase==true?true:false;//输入字符转小写(不用必填)zw2020-4-27


        this.listen()
        if(this.options.focused) {//从onfcues
            this.$element.focus();
        }

    }

    Autocomplete.prototype = {

        constructor: Autocomplete
    , processObj: 0
    , formatItem: function (item) {
        return item.toString();
    }
    , setValue: function (item) {
            //console.log("222:"+item.toString() + "--" + item.toString());
        return { "data-value": item.toString(), "real-value": item.toString() };
    }
    , select: function () {
        var ShowVal = this.$menu.find('.active').text();
        var val = this.$menu.find('.active').attr('data-value')
        var realVal = this.$menu.find('.active').attr('real-value')
        var json = $.parseJSON(this.$menu.find('.active').attr('json'));//返回原始数据Smart2017-6-7

        this.$element
          .val(this.updater(val)).attr("real-value", realVal)
          .change()
        this.$element.attr("title", ShowVal);//鼠标放上去显示的内容
        this.callback({ text: val, value: realVal,json:json });
        return this.hide()
    }
    , callback: function (item) {
        if (this.options.callback != undefined && typeof (this.options.callback) == "function") this.options.callback(item);//选中后执行回调函数(smart2013-10-24)
    }

    , updater: function (item) {
        return item
    }

    , show: function () {
        var pos = $.extend({}, this.$element.position(), {
            height: this.$element[0].offsetHeight
        })

        this.$menu
          .insertAfter(this.$element)
          .css({
              top: pos.top + pos.height
          , left: pos.left
          })
          .show()

        this.shown = true
        return this
    }
     , hideNew: function(_self,time) {
            //console.log("hide New222");
            var timeCount=time || 500;
            setTimeout(function () {
                _self.$menu.hide()
                _self.shown = false
            },timeCount);
        }
    , hide: function () {
        this.$menu.hide()
        this.shown = false
        return this
    }

    , lookup: function (event) {
        var items

        this.query = this.$element.val()

        //old
        //if (!this.query || this.query.length < this.options.minLength) {
        //    return this.shown ? this.hide() : this
        //}
        //new
        if (this.query.length < this.options.minLength) {
            return this.shown ? this.hide() : this
        }
        items = $.isFunction(this.source) ? this.source(this.query, $.proxy(this.process, this)) : this.source

        return items ? this.process(items) : this
    }

    , process: function (items) {
        var that = this
        if (!items.length) {
            return this.shown ? this.hide() : this
        }

        return this.render(items.slice(0, this.options.items)).show()
    }

    , highlighter: function (item) {
        var that = this
        item = that.formatItem(item)
        var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&')
        return item.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
            return '<strong style="color:#FF6600;">' + match + '</strong>'
        })
    }

    , render: function (items) {
        var that = this

        items = $(items).map(function (i, item) {
            i = $(that.options.item).attr(that.setValue(item))
            i.find('a').html(that.highlighter(item))
            return i[0]
        })

        items.first().addClass('active')
        this.$menu.html(items)
        return this
    }

    , next: function (event) {
        var active = this.$menu.find('.active').removeClass('active')
          , next = active.next()

        if (!next.length) {
            next = $(this.$menu.find('li')[0])
        }

        next.addClass('active')
    }

    , prev: function (event) {
        var active = this.$menu.find('.active').removeClass('active')
          , prev = active.prev()

        if (!prev.length) {
            prev = this.$menu.find('li').last()
        }

        prev.addClass('active')
    }

    , listen: function () {
        this.$element
          .on('focus', $.proxy(this.focus, this))
          .on('blur', $.proxy(this.blur, this))
          .on('keypress', $.proxy(this.keypress, this))
          .on('keyup', $.proxy(this.keyup, this))
          //.on('dblclick', $.proxy(this.dblclick, this))//双击自动弹出选择
        .on('click', $.proxy(this.dblclick, this))//单击自动弹出选择

        if (this.eventSupported('keydown')) {
            this.$element.on('keydown', $.proxy(this.keydown, this))
        }

        this.$menu
          .on('click', $.proxy(this.click, this))
          .on('mouseenter', 'li', $.proxy(this.mouseenter, this))
          .on('mouseleave', 'li', $.proxy(this.mouseleave, this))
    }

    , eventSupported: function (eventName) {
        var isSupported = eventName in this.$element
        if (!isSupported) {
            this.$element.setAttribute(eventName, 'return;')
            isSupported = typeof this.$element[eventName] === 'function'
        }
        return isSupported
    }

    , move: function (e) {
        if (!this.shown) return

        switch (e.keyCode) {
            //case 9: // tab  按tab会自动选择第项，不需要这个功能,smart2019-6-27
            case 13: // enter
            case 27: // escape
                e.preventDefault()
                break

            case 38: // up arrow
                e.preventDefault()
                this.prev()
                break

            case 40: // down arrow
                e.preventDefault()
                this.next()
                break
        }

        e.stopPropagation()
    }

    , keydown: function (e) {
        this.suppressKeyPressRepeat = ~$.inArray(e.keyCode, [40, 38, 9, 13, 27])
        this.move(e)
    }

    , keypress: function (e) {
        if (this.suppressKeyPressRepeat) return
        this.move(e)
    }

    , keyup: function (e) {
        switch (e.keyCode) {
            case 40: // down arrow
            case 38: // up arrow
            case 16: // shift
            case 17: // ctrl
            case 18: // alt
                break

            case 9: // tab
            case 13: // enter
                if (!this.shown) return
                this.select()
                break

            case 27: // escape
                if (!this.shown) return
                this.hide()
                break

            default:
                if(this.options.isUpperCase)this.$element.val(this.$element.val().toUpperCase());
                if(this.options.isLowerCase)this.$element.val(this.$element.val().toLowerCase());

                //this.$element.removeAttr("real-value").removeAttr("title"); //输入别的清除real-value值
                this.$element.attr("real-value", "").attr("title", "");//输入别的清除real-value值
                var that = this
                if (that.processObj) {
                    clearTimeout(that.processObj)
                    that.processObj = 0
                }
                that.processObj = setTimeout(function () {
                    that.lookup()
                }, that.options.delay)
                if (this.$element.val() == "") this.callback();//清除完后也要回调
        }

        e.stopPropagation()
        e.preventDefault()
    }
    , dblclick: function (e) {
        var that = this
        that.$element.select();//选中全选smart
        if (that.processObj) {
            clearTimeout(that.processObj)
            that.processObj = 0
        }
        that.processObj = setTimeout(function () {
            that.lookup()
        }, that.options.delay)
        e.stopPropagation()
        e.preventDefault()
    }
    , focus: function (e) {
        this.focused = true
        if (this.focused && !this.mousedover) this.dblclick(e);//一获得焦点就弹下拉框
        //$.proxy(this.dblclick, this)//smart2016-10-27注释这行，添加上面一行
    }

    , blur: function (e) {
            //console.log("=============blur================");
            //console.log("FF:"+this.freelyInput);
            //console.log("FF:"+this.options.freelyInput);

        var val = this.$element.val();
        var realVal = this.$element.attr('real-value') || "";
        //var realVal = val == "" ? this.$element.attr('real-value', '') : this.$element.attr('real-value', '') || "";
        // console.log("focused:" + this.focused);
        // console.log("mousedover:" + this.mousedover+"---shown:"+this.shown);
        // console.log("val:" + val+"---realVal:"+realVal);
        if(this.focused){
            if (val.length > 0 && realVal == "" && !this.mousedover && realVal != '0') {
                //console.log("LLL:"+this.freelyInput);
                //console.log("LLL:"+this.options.freelyInput);
                if(!this.freelyInput) {
                    var msgConfirm=msgConfirm="您当前还没有选择值，是否强制离开？";
                    if(typeof(CURLANG) != "undefined" && CURLANG=='en')msgConfirm="You have not yet select,Deselect or not ?";
                    if (confirm(msgConfirm)) {
                        this.focused = false;
                        this.$element.attr("real-value", "").val("").attr("title", "");//输入别的清除real-value值
                        //if (!this.mousedover && this.shown) this.hide();
                        if (!this.mousedover) this.hide();
                        this.callback();//清除也要回调
                    }else {
                        //取消，继续this.focused = true;
                        this.$element.focus();
                    }
                }else{
                    //console.log("自由输入");
                    this.focused = false
                    this.hide();
                    this.callback({text:val,value:null});//自由输入也要回调并
                }
            } else {
                // if (!this.mousedover && this.shown){
                //     this.hide()
                // }
                if (!this.mousedover) {
                    this.focused = false
                    if(this.shown){
                        this.hide();
                    }else{
                        //因为有时候下拉选择的菜单还没显示出来，但已经失焦。这时候需要延时隐藏
                        var _self = this;
                        this.hideNew(_self,500)
                    }
                }
            }
        }
    }

    , click: function (e) {

        e.stopPropagation()
        e.preventDefault()
        this.select()
        this.$element.focus()
    }

    , mouseenter: function (e) {
        this.mousedover = true
        this.$menu.find('.active').removeClass('active')
        $(e.currentTarget).addClass('active')
    }

    , mouseleave: function (e) {
        this.mousedover = false
        if (!this.focused && this.shown) this.hide()
    }

    }


    /* TYPEAHEAD PLUGIN DEFINITION
     * =========================== */

    var old = $.fn.autocomplete

    $.fn.autocomplete = function (option) {
        return this.each(function () {
            var $this = $(this)
              , data = $this.data('autocomplete')
              , options = typeof option == 'object' && option
            if (!data) $this.data('autocomplete', (data = new Autocomplete(this, options)))
            if (typeof option == 'string') data[option]()
        })
    }

    $.fn.autocomplete.defaults = {
        source: []
    , items: 8
    , menu: '<ul class="typeahead dropdown-menu"></ul>'
    , item: '<li><a href="#"></a></li>'
    , minLength: 0
    , delay: 500
    }

    $.fn.autocomplete.Constructor = Autocomplete


    /* TYPEAHEAD NO CONFLICT
     * =================== */

    $.fn.autocomplete.noConflict = function () {
        $.fn.autocomplete = old
        return this
    }


    /* TYPEAHEAD DATA-API
     * ================== */

    $(document).on('focus.autocomplete.data-api', '[data-provide="autocomplete"]', function (e) {
        var $this = $(this)
        if ($this.data('autocomplete')) return
        $this.autocomplete($this.data())
    })

}(window.jQuery);