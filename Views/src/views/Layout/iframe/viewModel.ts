export class ViewModel{
    public url:KnockoutObservable<string>;
    constructor(params, componentDef, $root){
        this.url = ko.observable();
        let urlId = ko.unwrap(params.url);
        $root.getLink(urlId).then((argUrl)=>{
            this.url(argUrl);
        });
    }
}