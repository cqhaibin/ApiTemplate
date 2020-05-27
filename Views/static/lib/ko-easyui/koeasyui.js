(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports) :
    typeof define === 'function' && define.amd ? define(['exports'], factory) :
    (factory((global.koeasyui = {})));
}(this, (function (exports) { 'use strict';

    var classCallCheck = function (instance, Constructor) {
      if (!(instance instanceof Constructor)) {
        throw new TypeError("Cannot call a class as a function");
      }
    };

    var createClass = function () {
      function defineProperties(target, props) {
        for (var i = 0; i < props.length; i++) {
          var descriptor = props[i];
          descriptor.enumerable = descriptor.enumerable || false;
          descriptor.configurable = true;
          if ("value" in descriptor) descriptor.writable = true;
          Object.defineProperty(target, descriptor.key, descriptor);
        }
      }

      return function (Constructor, protoProps, staticProps) {
        if (protoProps) defineProperties(Constructor.prototype, protoProps);
        if (staticProps) defineProperties(Constructor, staticProps);
        return Constructor;
      };
    }();

    var defineProperty = function (obj, key, value) {
      if (key in obj) {
        Object.defineProperty(obj, key, {
          value: value,
          enumerable: true,
          configurable: true,
          writable: true
        });
      } else {
        obj[key] = value;
      }

      return obj;
    };

    var inherits = function (subClass, superClass) {
      if (typeof superClass !== "function" && superClass !== null) {
        throw new TypeError("Super expression must either be null or a function, not " + typeof superClass);
      }

      subClass.prototype = Object.create(superClass && superClass.prototype, {
        constructor: {
          value: subClass,
          enumerable: false,
          writable: true,
          configurable: true
        }
      });
      if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass;
    };

    var possibleConstructorReturn = function (self, call) {
      if (!self) {
        throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
      }

      return call && (typeof call === "object" || typeof call === "function") ? call : self;
    };

    /**
     * 生成器的配置项
     */
    var GenerateOption = function GenerateOption(ko) {
        classCallCheck(this, GenerateOption);

        this.ko = ko;
        this.jquery = $;
    };

    /**
     * 组件基类
     */
    var BaseComp = function BaseComp(type) {
        classCallCheck(this, BaseComp);

        this.type = type;
    };

    /**
     * 返回指定数据不重复的记录
     * @param arrays
     */
    function unique(arrays) {
        var result = new Array();
        var hash = Object.create({});
        for (var irow = 0; irow < arrays.length; ++irow) {
            var elm = arrays[irow];
            if (!hash[elm]) {
                result.push(elm);
                hash[elm] = true;
            }
        }
        return result;
    }
    var CONSTKEY;
    (function (CONSTKEY) {
        /**
         * 组件名称前缀
         */
        CONSTKEY["CompPrefix"] = "ko-";
    })(CONSTKEY || (CONSTKEY = {}));
    /**
     * 根据组件名称获取模板DOM
     * @param compName
     */
    function getTemplate(compName) {
        var defaultVaue = '<div></div>';
        var specialValue = {
            'linkbutton': '<a></a>',
            'layout': '<div style="height:100%" ><!-- ko template:{ nodes:$componentTemplateNodes,data:$context.$parent } --><!-- /ko --></div>',
            'dialog': '<div><!-- ko template:{ nodes:$componentTemplateNodes,data:$context.$parent } --><!-- /ko --></div>'
        };
        var val = specialValue[compName];
        if (!val) {
            val = defaultVaue;
        }
        return val;
    }
    /**
     * 将arguments转换为数组
     */
    var convertArgToArray = Array.prototype.slice;
    /**
     * 组件类型
     */
    var COMPTYPE;
    (function (COMPTYPE) {
        /**
         * easyui实例组件
         */
        COMPTYPE[COMPTYPE["easyui"] = 1] = "easyui";
        /**
         * 普通组件类型
         */
        COMPTYPE[COMPTYPE["norm"] = 8] = "norm";
    })(COMPTYPE || (COMPTYPE = {}));

    /**
     * 所有easyui的基类
     */

    var BaseEasyuiComp = function (_BaseComp) {
        inherits(BaseEasyuiComp, _BaseComp);

        function BaseEasyuiComp(name, props, componentConfig) {
            classCallCheck(this, BaseEasyuiComp);

            var _this = possibleConstructorReturn(this, (BaseEasyuiComp.__proto__ || Object.getPrototypeOf(BaseEasyuiComp)).call(this, COMPTYPE.easyui));

            _this.props = props;
            _this.name = name;
            _this.$dom = $($(componentConfig.element)[0].firstChild);
            _this.$parent = _this.$dom.parent();
            var context = ko.contextFor(componentConfig.element);
            $.data(componentConfig.element, context);
            _this.koContext = context;
            return _this;
        }
        /**
         * 根据参数创建组件的配置对象（核心方法）
         * @param options 配置参数
         */


        createClass(BaseEasyuiComp, [{
            key: 'createOptionsCore',
            value: function createOptionsCore(options) {
                var _this2 = this;

                var opt = null;
                if (options) {
                    //viewModel不能转换
                    var viewModel = options.viewModel || null;
                    opt = ko.toJS(options);
                    if (viewModel) {
                        opt.viewModel = viewModel;
                    }
                    $.map(opt, function (value, key) {
                        if (key.substring(0, 2) == 'on' && $.isFunction(value)) {
                            //事件
                            opt[key] = value.bind(_this2);
                        }
                    });
                }
                return opt;
            }
            /**
             * 根据参数创建组件的配置对象
             * @param options 配置参数
             */

        }, {
            key: 'createOptions',
            value: function createOptions(options) {
                return this.createOptionsCore(options);
            }
            /**
             * 绘制组件
             * @param options
             */

        }, {
            key: 'paint',
            value: function paint(options) {
                var opt = this.createOptions(options);
                this.$dom[this.name](opt);
            }
            /**
            * 重置组件到初始化状态
            * @param options 配置项
            */

        }, {
            key: 'resetComp',
            value: function resetComp(options) {
                var $parent = this.$dom.closest('' + CONSTKEY.CompPrefix + this.name);
                if ($parent.length == 0) {
                    $parent = this.$parent;
                }
                //@ts-ignore
                this.destroy && this.destroy(this);
                var $dom = $(getTemplate(this.name));
                $parent.append($dom);
                this.$dom = $dom;
            }
        }]);
        return BaseEasyuiComp;
    }(BaseComp);

    var EasyuiMessager = function (_BaseEasyuiComp) {
        inherits(EasyuiMessager, _BaseEasyuiComp);

        function EasyuiMessager(params, componentConfig) {
            classCallCheck(this, EasyuiMessager);

            var _this = possibleConstructorReturn(this, (EasyuiMessager.__proto__ || Object.getPrototypeOf(EasyuiMessager)).call(this, 'messager', Object.getOwnPropertyNames($.messager.defaults), componentConfig));

            _this.bindMethods();
            return _this;
        }

        createClass(EasyuiMessager, [{
            key: 'bindMethods',
            value: function bindMethods() {
                var _this2 = this;

                var keys = this.getMethodKeys();
                $.map(keys, function (key) {
                    _this2[key] = function () {
                        var args = convertArgToArray.call(arguments);
                        $.messager[key].apply($.messager, args);
                    };
                });
            }
            /**
             * 获取方法的Key数据
             */

        }, {
            key: 'getMethodKeys',
            value: function getMethodKeys() {
                var methodKeys = new Array();
                $.map(Object.getOwnPropertyNames($.messager), function (key) {
                    if (key != 'defaults') {
                        methodKeys.push(key);
                    }
                });
                return methodKeys;
            }
        }, {
            key: 'paint',
            value: function paint(options) {
                var opts = this.createOptions(options);
                $.extend($.messager.defaults, opts);
            }
        }, {
            key: 'repaint',
            value: function repaint(options) {
                this.paint(options);
            }
        }]);
        return EasyuiMessager;
    }(BaseEasyuiComp);

    /**
     * 基于easyui's dialog的复式弹出窗口组件
     * modify: 2018-09-19 sam dargon add $key， 用于外界获取实例化的对话框
     */

    var EasyuiDialogs = function (_BaseComp) {
        inherits(EasyuiDialogs, _BaseComp);
        createClass(EasyuiDialogs, [{
            key: 'paint',
            value: function paint(options) {}
        }]);

        function EasyuiDialogs(params, componentConfig) {
            classCallCheck(this, EasyuiDialogs);

            var _this = possibleConstructorReturn(this, (EasyuiDialogs.__proto__ || Object.getPrototypeOf(EasyuiDialogs)).call(this, COMPTYPE.norm));

            _this.name = 'dialogs';
            _this.bindContext = ko.contextFor(componentConfig.element);
            _this.list = ko.observableArray();
            return _this;
        }

        createClass(EasyuiDialogs, [{
            key: 'repaint',
            value: function repaint(options) {}
            /**
             * 添加弹出窗口
             * @param params
             */

        }, {
            key: 'addDialog',
            value: function addDialog(params) {
                var options = params || {};
                var that = this;
                var key = 'dialogs_' + ko.unwrap(this.list).length;
                var funClose = options.onClose;
                options.onClose = function (key) {
                    return function () {
                        that.removeDialog(key, this.$dom[0]);
                        if (funClose) {
                            funClose.call(that, key);
                        }
                    };
                }(key);
                //add: 2018-09-19 sam dargon, remark: set $key, use getContextForDialog in app.
                options.$key = key;
                this.list.push({
                    name: 'ko-dialog',
                    params: {
                        options: options
                    },
                    key: key
                });
            }
            /**
             * 移除已弹出的窗口
             * @param key 需要移除的窗口的编号
             */

        }, {
            key: 'removeDialog',
            value: function removeDialog(key, dom) {
                ko.cleanNode(dom.firstElementChild);
                this.list.remove(function (item) {
                    return item.key === key;
                });
            }
        }], [{
            key: 'getTemplate',
            value: function getTemplate$$1() {
                return '<div data-bind="foreach:{data:list}" ><div data-bind="component:$data,attr:{id:(\'dialogs_\' + $index())}" ></div></div>';
            }
        }]);
        return EasyuiDialogs;
    }(BaseComp);

    var EasyuiHelper = function () {
        function EasyuiHelper() {
            classCallCheck(this, EasyuiHelper);
        }

        createClass(EasyuiHelper, null, [{
            key: 'createEasyui',
            value: function createEasyui(compName, props, methods, execute, extendBind) {
                var tmpClass = function (_BaseEasyuiComp) {
                    inherits(tmpClass, _BaseEasyuiComp);

                    function tmpClass(params, componentConfig) {
                        classCallCheck(this, tmpClass);

                        var _this = possibleConstructorReturn(this, (tmpClass.__proto__ || Object.getPrototypeOf(tmpClass)).call(this, compName.toLowerCase(), props, componentConfig));

                        _this.params = params;
                        if (!_this.params.options) {
                            _this.params.options = {};
                        }
                        _this.$dom = $($(componentConfig.element)[0].firstChild);
                        var that = _this;
                        //绑定方法，方法还需要继承组件支持的方法的绑定
                        $.map(Object.getOwnPropertyNames(methods), function (index) {
                            if (!$.isNumeric(index)) return true;
                            var methodName = methods[index];
                            that[methodName] = function () {
                                //需要加一个拦截机制，用于处理依赖组件的方法（如：combogrid，依赖grid，那么grid的方法怎么执行）
                                var args = convertArgToArray.call(arguments);
                                //传入的参数进行扩展
                                extendBind.extend(that.name, args[0], that.koContext);
                                args.unshift(methodName);
                                return execute.execute(that.name, methodName, that.$dom, args);
                            };
                        });
                        return _this;
                    }
                    /**
                     * 绑定change事件
                     */


                    createClass(tmpClass, [{
                        key: 'bindChangeEvent',
                        value: function bindChangeEvent(sourceopts, opt) {
                            var changeEvent = false;
                            $.map(props, function (item, key) {
                                if (item == 'onChange') {
                                    changeEvent = true;
                                    return false;
                                }
                            });
                            if (sourceopts.value && ko.isObservable(sourceopts.value) && changeEvent) {
                                var onChangeFn = opt.onChange;
                                var that = this;
                                opt['onChange'] = function (newValue, oldValue) {
                                    //modify: 2018-10-10 sam dragon set value, but don't send notification.
                                    // and add onChange event.
                                    sourceopts.value._latestValue = newValue;
                                    onChangeFn && onChangeFn.call(that, newValue, oldValue);
                                };
                            }
                        }
                    }, {
                        key: 'createOptions',
                        value: function createOptions(options) {
                            var opt = this.createOptionsCore(options);
                            if (options) {
                                this.bindChangeEvent(options, opt);
                            }
                            extendBind.extend(this.name, opt, this.koContext);
                            return opt;
                        }
                    }, {
                        key: 'repaint',
                        value: function repaint(options) {
                            this.resetComp(options);
                            this.paint(options);
                        }
                    }, {
                        key: 'dispose',
                        value: function dispose() {
                            console.log('dispose ' + this.name);
                            //@ts-ignore
                            if (this.computedObservable) {
                                //@ts-ignore
                                this.computedObservable.dispose();
                            }
                            if (this.params.options.dispose) {
                                this.params.options.dispose.apply(this);
                            } else {
                                //@ts-ignore
                                this.destroy && this.destroy();
                            }
                        }
                    }]);
                    return tmpClass;
                }(BaseEasyuiComp);
                return tmpClass;
            }
        }]);
        return EasyuiHelper;
    }();

    /**
     * 配置与方法的关系
     * 确定那些配置的改变，可以通过方法实现
     */
    var relation = {
        tooltip: {
            'content': 'update'
        },
        textbox: {
            'value': 'setValue'
        },
        panel: {
            'title': 'setTitle'
        },
        passwordbox: {
            'value': 'setValue'
        },
        combo: {
            'value': 'setValue'
        },
        combobox: {
            'value': 'setValue',
            'data': 'loadData'
        },
        combotree: {
            'value': 'setValue',
            'data': 'loadData'
        },
        combogrid: {
            'value': 'setValue',
            'data': 'loadData'
        },
        combotreegrid: {
            'value': 'setValue',
            'data': 'loadData'
        },
        tagbox: {
            'value': 'setValue',
            'data': 'loadData'
        },
        numberbox: {
            'value': 'setValue'
        },
        datebox: {
            'value': 'setValue'
        },
        datetimebox: {
            'value': 'setValue'
        },
        datetimespinner: {
            'value': 'setValue'
        },
        spinner: {
            'value': 'setValue'
        },
        numberspinner: {
            'value': 'setValue'
        },
        timespinner: {
            'value': 'setValue'
        },
        slider: {
            'value': 'setValue'
        },
        filebox: {
            'value': 'setValue'
        },
        datagrid: {
            'data': 'loadData'
        },
        datalist: {
            'data': 'loadData'
        },
        propertygrid: {
            'data': 'loadData'
        },
        tree: {
            'data': 'loadData'
        },
        treegrid: {
            'data': 'loadData'
        }
    };

    /**
     * easyui控件依赖关系
     */
    var depents = {
        pagination: { dependencies: ['linkbutton'] },
        datagrid: { dependencies: ['panel', 'resizable', 'linkbutton', 'pagination'] },
        treegrid: { dependencies: ['datagrid'] },
        propertygrid: { dependencies: ['datagrid'] },
        datalist: { dependencies: ['datagrid'] },
        window: { dependencies: ['resizable', 'draggable', 'panel'] },
        dialog: { dependencies: ['linkbutton', 'window'] },
        messager: { dependencies: ['linkbutton', 'window', 'progressbar'] },
        layout: { dependencies: ['resizable', 'panel'] },
        tabs: { dependencies: ['panel', 'linkbutton'] },
        menubutton: { dependencies: ['linkbutton', 'menu'] },
        splitbutton: { dependencies: ['menubutton'] },
        accordion: { dependencies: ['panel'] },
        textbox: { dependencies: ['validatebox', 'linkbutton'] },
        filebox: { dependencies: ['textbox'] },
        combo: { dependencies: ['panel', 'textbox'] },
        combobox: { dependencies: ['combo'] },
        combotree: { dependencies: ['combo', 'tree'] },
        combogrid: { dependencies: ['combo', 'datagrid'] },
        combotreegrid: { dependencies: ['combo', 'treegrid'] },
        validatebox: { dependencies: ['tooltip'] },
        numberbox: { dependencies: ['textbox'] },
        searchbox: { dependencies: ['menubutton', 'textbox'] },
        spinner: { dependencies: ['textbox'] },
        numberspinner: { dependencies: ['spinner', 'numberbox'] },
        timespinner: { dependencies: ['spinner'] },
        tree: { dependencies: ['draggable', 'droppable'] },
        datebox: { dependencies: ['calendar', 'combo'] },
        datetimebox: { dependencies: ['datebox', 'timespinner'] },
        slider: { dependencies: ['draggable'] }
    };
    /**
     * 获取依赖项
     * @param name
     */
    function getDepend(name) {
        var deps = depents[name];
        var depret = [];
        if (deps && deps.dependencies) {
            $.map(deps.dependencies, function (d) {
                if (depents[d]) {
                    depret = depret.concat(getDepend(d));
                }
                depret.push(d);
            });
        }
        return depret;
    }
    /**
     * 根据组件名、方法名，获取此方法归属组件（因为方法有可能归属于依赖组件）
     * @param componentName 组件名
     * @param methodName 方法名
     * @param jquery jquery对象
     */
    function getDependNameOfComponent(componentName, methodName, argdeps, jquery) {
        var methods = jquery.fn[componentName].methods;
        var result = null;
        $.map(Object.getOwnPropertyNames(methods), function (m) {
            if (m == methodName) {
                result = componentName;
                return false;
            }
        });
        if (!result) {
            var deps = depents[componentName] ? depents[componentName].dependencies : null;
            if (deps) {
                $.map(deps, function (d) {
                    if (!result) {
                        var tmp = getDependNameOfComponent(d, methodName, argdeps, jquery);
                        if (tmp && argdeps) {
                            //如果查询到的组件名在依赖列表内，可使用
                            var depkeys = Object.getOwnPropertyNames(argdeps).join(',');
                            if (depkeys.indexOf(tmp) >= 0) {
                                result = tmp;
                            }
                        }
                    }
                });
            }
        }
        return result;
    }

    /**
     * 获取可执行方法（带依赖检测）
     */

    var DepCheckExecute = function () {
        function DepCheckExecute(jquery) {
            classCallCheck(this, DepCheckExecute);

            this.jquery = jquery;
            this.deps = {
                'combo': {
                    'textbox': 'textbox'
                },
                'combogrid': {
                    'datagrid': 'grid'
                },
                'combotreegrid': {
                    'treegrid': 'grid'
                },
                'combotree': {
                    'tree': 'tree'
                },
                'datagrid': {
                    'panel': 'getPanel',
                    'pagination': 'getPager'
                },
                'propertygrid': {
                    'panel': 'getPanel'
                },
                'datetimebox': {
                    'spinner': 'spinner'
                },
                'datebox': {
                    'calendar': 'calendar'
                },
                'searchbox': {
                    'textbox': 'textbox',
                    'menu': 'menu'
                },
                'datalist': {
                    'panel': 'getPanel',
                    'pagination': 'getPager'
                }
            };
            this.noSupportDestroy = ['layout', 'tabs'];
        }
        /**
         * 获取可执行方法（带依赖检测）
         */


        createClass(DepCheckExecute, [{
            key: 'execute',
            value: function execute(compName, methodName, $dom, args) {
                var findComp = false;
                $.map(this.noSupportDestroy, function (item, key) {
                    if (item == compName) {
                        findComp = true;
                        return false;
                    }
                });
                if (methodName == 'destroy' && findComp) {
                    return;
                }
                //have bug: 如我执行分页的select方法，很有可能会调用linkbutton的select。依赖组件之间有重名方法时，有问题
                var depCompName = getDependNameOfComponent(compName, methodName, this.deps[compName], this.jquery);
                if (depCompName && depCompName != compName && this.deps[compName]) {
                    //依赖上的
                    var depHandler = $dom[compName].apply($dom, [this.deps[compName][depCompName]]);
                    var $depDom = this.jquery(depHandler[0]);
                    return depHandler[depCompName].apply($depDom, args);
                }
                return $dom[compName].apply($dom, args);
            }
        }]);
        return DepCheckExecute;
    }();

    /**
     * 提供获取的可执行的入口
     */

    var ExecuteProivder = function () {
        function ExecuteProivder(opt) {
            classCallCheck(this, ExecuteProivder);

            this.opt = opt;
            this.executes = new DepCheckExecute(this.opt.jquery);
        }

        createClass(ExecuteProivder, [{
            key: 'execute',
            value: function execute(compName, methodName, $dom, args) {
                return this.executes.execute(compName, methodName, $dom, args);
            }
        }]);
        return ExecuteProivder;
    }();

    var DefaultGenerate = function () {
        function DefaultGenerate(option) {
            classCallCheck(this, DefaultGenerate);

            this.option = option;
        }

        createClass(DefaultGenerate, [{
            key: 'generate',
            value: function generate(componentName, params, viewModel) {
                var first = true,
                    that = this;
                this._touchMount(componentName, params, viewModel);
                var observableKeys = this._getObservableKeys(params);
                //配置参数存在，就进行监听
                if (params.options) {
                    //监听params的变化变化
                    ko.computed(function () {
                        var changeOpts = that._getChangeObservable(params, observableKeys, viewModel);
                        if (first) {
                            //如果是初始化执行，后面的业务不用重复执行了
                            first = false;
                            return;
                        }
                        if (changeOpts.length > 0) {
                            that._touchPaint(componentName, params, viewModel, changeOpts, function () {
                                //引起了组件重绘
                                viewModel.repaint && viewModel.repaint(params.options);
                            });
                        }
                    });
                }
                return viewModel;
            }
            /**
            * 内部方法，获取配置参数(options)上的所有可监控对象的key，首次渲染组件时执行
            * @param params 组件的所有参数
            */

        }, {
            key: '_getObservableKeys',
            value: function _getObservableKeys(params) {
                var keys = new Array();
                var opts = params.options;
                if (!opts) return keys;
                $.map(Object.getOwnPropertyNames(opts), function (key) {
                    var param = opts[key];
                    if (ko.isObservable(param)) {
                        keys.push(key);
                    }
                });
                return keys;
            }
            /**
             * 获取有变更的监控对象列表
             * @param params 配置对象
             * @param observableKeys 可监控的参数列表key
             * @param viewModel 视图数据对象
             * @return
             * {
             *  changeOpts：存在变更的对象列表
             *  reflows：可以使用回流操作来处理的变更列表
             * }
             */

        }, {
            key: '_getChangeObservable',
            value: function _getChangeObservable(params, observableKeys, viewModel) {
                var opts = params.options;
                var changeOpts = new Array();
                $.map(observableKeys, function (key) {
                    var param = opts[key];
                    var tmp = ko.unwrap(param);
                    //探测监控对象有变化的属性，区分那些可以用方法进行改变，那些需要重绘
                    if (ko.isObservable(param) && param.hasChanged()) {
                        changeOpts.push(param);
                    }
                });
                return changeOpts;
            }
            /**
             * 触发挂载方法，以及相关勾子
             * @param componentName 组件名称
             * @param params 参数列表
             * @param viewModel 视图数据对象
             */

        }, {
            key: '_touchMount',
            value: function _touchMount(componentName, params, viewModel) {
                var canPaint = true;
                if (viewModel.beforeMount) {
                    //绘制前的勾子
                    canPaint = viewModel.beforeMount(params, componentName);
                }
                {
                    //阻止子组件的渲染？？？怎么办
                    viewModel.paint && viewModel.paint(params.options || {});
                    if (viewModel.afterMount) {
                        //绘制后的勾子
                        viewModel.afterMount(params, componentName);
                    }
                }
            }
            /**
             * 触发组件的重绘
             * @param componentName 组件名称
             * @param params 参数列表
             * @param viewModel 视图数据对象
             * @param changeOpts 存在变更的可监控列表
             * @param action beforePaint与afterPaint之间需要执行的动作
             */

        }, {
            key: '_touchPaint',
            value: function _touchPaint(componentName, params, viewModel, changeOpts, action) {
                var canPaint = void 0;
                if (viewModel.beforePaint) {
                    canPaint = viewModel.beforePaint(params, componentName, changeOpts);
                }
                {
                    action && action();
                    if (viewModel.afterPaint) {
                        viewModel.afterPaint(params, componentName);
                    }
                }
            }
        }]);
        return DefaultGenerate;
    }();

    /**
     * 扩展对window, dialog, panel, Layout, tabs, accordion控件内容可绑定ko组件的能力
     */
    var ExtendBind = function () {
        function ExtendBind(option) {
            classCallCheck(this, ExtendBind);

            /**
             * 获取可扩展的事件名称
             */
            this._getEventName = function (name) {
                var eventName = null;
                switch (name) {
                    case "window":
                    case "dialog":
                    case "layout":
                    case "panel":
                    //modify: 2018-10-08 sam dragon remove onAdd in tabs and accordion, and add listen onBeforeOpen in tabs and accordion.
                    case "tabs":
                    case "accordion":
                        eventName = "onBeforeOpen";
                        break;
                }
                return eventName;
            };
            this.option = option;
            this.jquery = option.jquery;
            this.ko = option.ko;
        }

        createClass(ExtendBind, [{
            key: "extend",
            value: function extend(name, opts, bindContext) {
                var eName = this._getEventName(name);
                if (eName) {
                    this._extendOption(eName, opts, bindContext);
                }
            }
            /**
             * 将指定的事件扩展为支持ko组件自动绑定的能力
             * @param eName 事件名称
             * @param opts 配置参数
             * @param bindContext ko组件上下文
             */

        }, {
            key: "_extendOption",
            value: function _extendOption(eName, opts, bindContext) {
                var that = this;
                if (opts && opts.viewModel) {
                    //说明有ko组件需要绑定
                    var _orgEvent = opts[eName];
                    var childBindContext = void 0; //绑定对象的创建
                    opts[eName] = function () {
                        //modify: 2018-10-08 sam dragon 处理tabs、accordion组件选项切换引起的Bug。
                        if (childBindContext) {
                            ko.removeNode(this.firstChild);
                            $(this).append(opts.content);
                        }
                        //viewModel必须是对象
                        childBindContext = bindContext.createChildContext(opts.viewModel, undefined, function (ctx) {});
                        //@ts-ignore
                        that.ko.applyBindingsToDescendants(childBindContext, this); //调用者，肯定是组件
                        _orgEvent && _orgEvent.apply(childBindContext.$data, arguments); //执行开发者配置的事件
                    };
                }
            }
        }]);
        return ExtendBind;
    }();

    ///<reference types="../../types/easyui" />

    var EasyuiGenerate = function (_DefaultGenerate) {
        inherits(EasyuiGenerate, _DefaultGenerate);

        function EasyuiGenerate(option) {
            classCallCheck(this, EasyuiGenerate);

            var _this = possibleConstructorReturn(this, (EasyuiGenerate.__proto__ || Object.getPrototypeOf(EasyuiGenerate)).call(this, option));

            _this.jquery = option.jquery;
            _this.easyui = _this.jquery.parser;
            _this.executeProvider = new ExecuteProivder(option);
            _this.extendBind = new ExtendBind(option);
            _this.registerPlugins();
            return _this;
        }
        /**
         * 组件easyui的相关组件
         */


        createClass(EasyuiGenerate, [{
            key: 'registerPlugins',
            value: function registerPlugins() {
                var _this2 = this;

                //fix easyui defaults
                //1. datagrid missing columns, frozenColumns;
                this.jquery.fn.datagrid.defaults.columns = [[]];
                this.jquery.fn.datagrid.defaults.frozenColumns = [[]];
                //extends panel default event.
                this._extendPanelEvent();
                var plugins = this.easyui.plugins;
                //动态生成一个function的类
                $.map(plugins, function (pluginName) {
                    var defaults$$1 = _this2.jquery.fn[pluginName].defaults;
                    var methods = _this2.jquery.fn[pluginName].methods;
                    var deps = getDepend(pluginName);
                    if (defaults$$1) {
                        //options必须要是独立的，事件（放原型上），方法可以原型链上的
                        var props = Object.getOwnPropertyNames(defaults$$1);
                        //方法
                        var methodKeys = Object.getOwnPropertyNames(methods);
                        if (deps) {
                            $.map(deps, function (dep) {
                                var depMethods = _this2.jquery.fn[dep].methods;
                                methodKeys = methodKeys.concat(Object.getOwnPropertyNames(depMethods));
                            });
                            methodKeys = unique(methodKeys);
                        }
                        _this2.option.ko.components.register('' + CONSTKEY.CompPrefix + pluginName, {
                            template: getTemplate(pluginName),
                            viewModel: EasyuiHelper.createEasyui(pluginName, props, methodKeys, _this2.executeProvider, _this2.extendBind)
                        });
                    }
                });
            }
        }, {
            key: 'generate',
            value: function generate(componentName, params, viewModel) {
                var first = true,
                    that = this;
                this._touchMount(componentName, params, viewModel);
                //获取监控数据列表
                var observableKeys = this._getObservableKeys(params);
                //配置参数存在，就进行监听
                if (params.options) {
                    //监听params的变化变化
                    //@ts-ignore
                    viewModel.computedObservable = ko.computed({
                        read: function read() {
                            //todo: 无意义，ko比较的是否判断，是需要传入一个比较的版本号的，每个属性都是有改动
                            var changeObs = that._getChangeObservable(params, observableKeys, viewModel);
                            if (first) {
                                //如果是初始化执行，后面的业务不用重复执行了
                                first = false;
                                return;
                            }
                            if (changeObs.changeOpts.length > 0) {
                                that._touchPaint(componentName, params, viewModel, changeObs, function () {
                                    if (changeObs.changeOpts.length == changeObs.reflows.length) {
                                        //说明配置的改变，可能通过方法操作完成
                                        $.map(changeObs.reflows, function (item) {
                                            viewModel[item.methodName](item.val);
                                        });
                                    } else {
                                        //引起了组件重绘
                                        viewModel.repaint(params.options);
                                    }
                                });
                            }
                        },
                        disposeWhenNodeIsRemoved: true
                    });
                }
                return viewModel;
            }
            /**
             * 获取有变更的监控对象列表
             * @param params 配置对象
             * @param observableKeys 可监控的参数列表key
             * @param viewModel 视图数据对象
             * @return
             * {
             *  changeOpts：存在变更的对象列表
             *  reflows：可以使用回流操作来处理的变更列表
             * }
             */

        }, {
            key: '_getChangeObservable',
            value: function _getChangeObservable(params, observableKeys, viewModel) {
                var opts = params.options;
                var changeOpts = new Array();
                var reflows = new Array(); //可以通过方法来进行配置改变的参数
                $.map(observableKeys, function (key) {
                    var param = opts[key];
                    var tmp = ko.unwrap(param);
                    //探测监控对象有变化的属性，区分那些可以用方法进行改变，那些需要重绘
                    if (ko.isObservable(param) && param.hasChanged()) {
                        changeOpts.push(param);
                        if (relation[viewModel.name] && relation[viewModel.name][key]) {
                            reflows.push({
                                val: tmp,
                                methodName: relation[viewModel.name][key]
                            });
                        }
                    }
                });
                return {
                    changeOpts: changeOpts,
                    reflows: reflows
                };
            }
            /**
             * modify:2018-06-05 sam dragon, 重写panel的onOpen方法，用于panel内容的组件解析
             * modify:2018-09-30 sam dragon, linkbutton增加destroy方法，用于销毁组件
             */

        }, {
            key: '_extendPanelEvent',
            value: function _extendPanelEvent() {
                $.fn.panel.defaults.onOpen = function () {};
                //@ts-ignore
                $.fn.linkbutton.methods.destroy = function ($dom) {
                    $dom.unbind('.linkbutton');
                    $dom.remove();
                };
            }
        }]);
        return EasyuiGenerate;
    }(DefaultGenerate);

    var EasyuiHelper$1 = function () {
        function EasyuiHelper() {
            classCallCheck(this, EasyuiHelper);
        }

        createClass(EasyuiHelper, null, [{
            key: 'createEasyui',
            value: function createEasyui(compName, props, methods, execute, extendBind) {
                var tmpClass = function (_BaseEasyuiComp) {
                    inherits(tmpClass, _BaseEasyuiComp);

                    function tmpClass(params, componentConfig) {
                        classCallCheck(this, tmpClass);

                        var _this = possibleConstructorReturn(this, (tmpClass.__proto__ || Object.getPrototypeOf(tmpClass)).call(this, compName.toLowerCase(), props, componentConfig));

                        _this.params = params;
                        if (!_this.params.options) {
                            _this.params.options = {};
                        }
                        _this.$dom = $($(componentConfig.element)[0].firstChild);
                        var that = _this;
                        //绑定方法，方法还需要继承组件支持的方法的绑定
                        $.map(Object.getOwnPropertyNames(methods), function (index) {
                            if (!$.isNumeric(index)) return true;
                            var methodName = methods[index];
                            that[methodName] = function () {
                                //需要加一个拦截机制，用于处理依赖组件的方法（如：combogrid，依赖grid，那么grid的方法怎么执行）
                                var args = convertArgToArray.call(arguments);
                                //传入的参数进行扩展
                                extendBind.extend(that.name, args[0], that.koContext);
                                args.unshift(methodName);
                                return execute.execute(that.name, methodName, that.$dom, args);
                            };
                        });
                        return _this;
                    }
                    /**
                     * 绑定change事件
                     */


                    createClass(tmpClass, [{
                        key: 'bindChangeEvent',
                        value: function bindChangeEvent(sourceopts, opt) {
                            var changeEvent = false;
                            $.map(props, function (item, key) {
                                if (item == 'onChange') {
                                    changeEvent = true;
                                    return false;
                                }
                            });
                            if (sourceopts.value && ko.isObservable(sourceopts.value) && changeEvent) {
                                var onChangeFn = opt.onChange;
                                var that = this;
                                opt['onChange'] = function (newValue, oldValue) {
                                    //modify: 2018-10-10 sam dragon set value, but don't send notification.
                                    // and add onChange event.
                                    sourceopts.value._latestValue = newValue;
                                    onChangeFn && onChangeFn.call(that, newValue, oldValue);
                                };
                            }
                        }
                    }, {
                        key: 'createOptions',
                        value: function createOptions(options) {
                            var opt = this.createOptionsCore(options);
                            if (options) {
                                this.bindChangeEvent(options, opt);
                            }
                            extendBind.extend(this.name, opt, this.koContext);
                            return opt;
                        }
                    }, {
                        key: 'repaint',
                        value: function repaint(options) {
                            this.resetComp(options);
                            this.paint(options);
                        }
                    }, {
                        key: 'dispose',
                        value: function dispose() {
                            console.log('dispose ' + this.name);
                            //@ts-ignore
                            if (this.computedObservable) {
                                //@ts-ignore
                                this.computedObservable.dispose();
                            }
                            if (this.params.options.dispose) {
                                this.params.options.dispose.apply(this);
                            } else {
                                //@ts-ignore
                                this.destroy && this.destroy();
                            }
                        }
                    }]);
                    return tmpClass;
                }(BaseEasyuiComp);
                return tmpClass;
            }
        }]);
        return EasyuiHelper;
    }();
    /**
     * 根据dom获取上下文
     * @param dom dom节点
     */


    function getContextFor$1(dom) {
        var comp = $.data(dom);
        return comp ? comp.$data : null;
    }

    function use(ko) {
        ko.components.register(CONSTKEY.CompPrefix + 'messager', {
            viewModel: EasyuiMessager,
            template: '<div></div>'
        });
        //dialogs
        ko.components.register(CONSTKEY.CompPrefix + 'dialogs', {
            viewModel: EasyuiDialogs,
            template: EasyuiDialogs.getTemplate()
        });
    }

    /**
     * 生成ko组件，并注册到ko组件对象中
     */

    var GenerateFactory = function () {
        function GenerateFactory(option) {
            var _generates;

            classCallCheck(this, GenerateFactory);

            this.option = option;
            this.generates = (_generates = {}, defineProperty(_generates, COMPTYPE.easyui, new EasyuiGenerate(this.option)), defineProperty(_generates, COMPTYPE.norm, new DefaultGenerate(this.option)), _generates);
            use(this.option.ko); //引用ko-messager组件
        }

        createClass(GenerateFactory, [{
            key: 'generate',
            value: function generate(componentName, params, viewModel) {
                var generateHandler = this.generates[viewModel.type || COMPTYPE.norm];
                if (generateHandler) {
                    return generateHandler.generate(componentName, params, viewModel);
                }
                return viewModel;
            }
        }]);
        return GenerateFactory;
    }();

    /**
     * ko的自定义loader，用于创建ko-easyui组件的生命周期
     */
    var EasyuiLoader = function () {
        function EasyuiLoader(factory) {
            classCallCheck(this, EasyuiLoader);

            this.factory = factory;
        }

        createClass(EasyuiLoader, [{
            key: 'getConfig',
            value: function getConfig(name, callback) {
                callback(null);
            }
        }, {
            key: 'loadComponent',
            value: function loadComponent(name, componentConfig, callback) {
                callback(null);
            }
        }, {
            key: 'loadTemplate',
            value: function loadTemplate(name, templateConfig, callback) {
                //这里做一些视图不显示的控制，在渲染数据后，进行视频的展示
                callback(null);
            }
        }, {
            key: 'loadViewModel',
            value: function loadViewModel(name, viewModelConfig, callback) {
                var _this = this;

                //到这里，视图都是已经呈现好的
                //这里要产生两个生命周期：渲染数据前、渲染数据后，以及一个视图重绘的事件
                var nViewModelConfig = function nViewModelConfig(params, componentConfig) {
                    //获取root对象
                    var $obj = ko.contextFor(componentConfig.element);
                    var $root = $obj.$root;
                    var vm = new viewModelConfig(params, componentConfig, $root, $obj.$data); //current $data is parent.
                    //激活
                    if (vm.activate) {
                        vm.activate(params, componentConfig);
                    }
                    var that = _this;
                    vm.koDescendantsComplete = function (dom) {
                        //组件绑定完成后的事件
                        //写入Context到自定义元素dom上
                        $.data(dom, ko.contextFor(dom.firstChild));
                        that.factory.generate(name, params, vm);
                    };
                    return vm;
                };
                callback(nViewModelConfig);
            }
        }]);
        return EasyuiLoader;
    }();

    var Components = function (_BaseComp) {
        inherits(Components, _BaseComp);

        function Components(params) {
            classCallCheck(this, Components);

            var _this = possibleConstructorReturn(this, (Components.__proto__ || Object.getPrototypeOf(Components)).call(this, COMPTYPE.norm));

            _this.comps = ko.observableArray();
            _this.paramComp = params.comp;
            if (_this.paramComp) {
                _this.paramComp.subscribe(function (nVal) {
                    var compInfo = {
                        id: 0,
                        name: null,
                        params: {},
                        show: ko.observable(false)
                    };
                    if (typeof nVal == 'string') {
                        compInfo.name = nVal;
                    } else {
                        compInfo.name = nVal.name;
                        compInfo.params = nVal.params || {};
                        compInfo.id = nVal.id || 0;
                    }
                    //@ts-ignore, the first read, execute fun at param in computed.
                    compInfo.domId = ko.computed(function () {
                        return 'components-item' + this.id;
                    }, compInfo);
                    _this._setComp(compInfo);
                });
            }
            return _this;
        }

        createClass(Components, [{
            key: '_setComp',
            value: function _setComp(compinfo) {
                var comp;
                $.each(ko.unwrap(this.comps), function (i, v) {
                    v.show(false);
                    if (v.name == compinfo.name && v.id == compinfo.id) {
                        comp = v;
                    }
                });
                if (!comp) {
                    comp = compinfo;
                    this.comps.push(comp);
                }
                comp.show(true);
            }
        }, {
            key: 'paint',
            value: function paint(options) {}
        }, {
            key: 'repaint',
            value: function repaint(options) {}
        }, {
            key: 'remove',
            value: function remove(arg) {
                var comp;
                var isObj = Object.prototype.toString.call(arg) == '[object String]' ? false : true;
                $.each(ko.unwrap(this.comps), function (i, v) {
                    //modify: 2019-07-11 sam  remark: arg is object or string
                    if (isObj && arg.name == v.name && arg.id == v.id || !isObj && v.name == arg) {
                        comp = v;
                    }
                });
                if (comp) {
                    comp.show(false);
                    this.comps.remove(comp);
                }
            }
        }, {
            key: 'removeAll',
            value: function removeAll() {
                this.comps.removeAll();
            }
        }], [{
            key: 'getTemplate',
            value: function getTemplate$$1() {
                return '<div class="components-list" data-bind="foreach:comps" ><div class="components-item" data-bind="component:$data,visible:show,attr:{id:$data.domId}" ></div></div>';
            }
        }]);
        return Components;
    }(BaseComp);

    /**
     * 主体思路
     * 1. 获取所有easyui的组件，以及组件的相关事件、配置参数、方法等
     * 2. easyui组件 映射为 ko-easyui组件，新组件名为 ko-easyui原组件名
     * 3. 做渲染钩子，建立一个组件生命周期的钩子
     * 4. 组件注册到ko对象上
     */
    /**
     * 插件注册入口
     * @param ko
     */
    function use$1(ko) {
      var opt = new GenerateOption(ko);
      var factory = new GenerateFactory(opt);
      ko.components.loaders.unshift(new EasyuiLoader(factory));
      //ko-components
      ko.components.register(CONSTKEY.CompPrefix + 'components', {
        viewModel: Components,
        template: Components.getTemplate()
      });
    }

    exports.use = use$1;
    exports.getContextFor = getContextFor$1;

    Object.defineProperty(exports, '__esModule', { value: true });

})));
//# sourceMappingURL=koeasyui.js.map
