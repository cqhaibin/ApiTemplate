
export class RouteInfo{
    public text:string;
    /**
     * 组件名称
     */
    public common:string;
    /**
     * 组件配置参数
     */
    public params:any;
    public route:string; 
    public group:string;
    /**
     * 组件渲染位置
     */
    public renderDom:string;
    public href:string;
    public parent:RouteInfo;
    public id:string;
    constructor(id:string, route:string, text:string,  group:string, common:string, renderDom?:string, params?:any){
        this.id = id;
        this.route = route;
        this.text = text;
        this.common = common;
        this.params = params || { options: {} };
        this.group = group;
        this.renderDom = renderDom || '';
        this.parent = null;
    }
}