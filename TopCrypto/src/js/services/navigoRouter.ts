import { IStart, IDisposable, ITemplate } from "./../interfaces/irootbundle";

declare let Navigo: any;

class NavigoRouter {
    lastInitializer = null;
    router = null;

    constructor() {
        this.router = new Navigo(location.origin);
        this.lastInitializer = null;
    }

    private _initializePage(initFunction: new (...any) => ITemplate, args: any) {
        this.lastInitializer && this.lastInitializer.dispose();

        let argsId = args ? args.id : null;
        let pageInitializer = <ITemplate>new initFunction(argsId, args);
        this.lastInitializer = pageInitializer;

        let promise = pageInitializer.onStart();

        let resizeFn = () => { $(window).resize(); };
        promise ? promise.then(resizeFn) : resizeFn();
    };

    public navigate(route: string) {
        this.router.navigate(route);
    }    

    public addRoot(link: string, initFunction: new () => ITemplate) {
        let self = this;

        this.router.on(link, function (params) {            
            self._initializePage(initFunction, params);
            self._initializeTitle('link_to_title_' + link);
        });       
    }

    public _initializeTitle(link: string) {

        if (!(<any>window).linkToTitles) return;
        let head = document.getElementsByTagName("head")[0];
        let title = head.getElementsByTagName("title")[0];

        let items = (<Array<{ id: string, value: string }>>(<any>window).linkToTitles)
            .filter((x) => { return x.id == link || x.id == "link_to_title_site_title_first_part"; });

        if (items) {
            let firstPartTitle = items.find((x) => { return x.id == "link_to_title_site_title_first_part"; });
            let item = items.find((x) => { return x.id == link; });

            if (firstPartTitle && firstPartTitle.value) {
                if (item && item.value) {
                    title.textContent = `${firstPartTitle.value}-${item.value}`;
                } else {
                    title.textContent = `${firstPartTitle.value}`;
                }
            } else if (item && item.value) {
                title.textContent = `${item.value}`;
            }
        }
    }

    public addSimpleRoot(link: string, initFunction: Function) {
        this.router.on(link, initFunction);
    }

    public resolve() {
        this.router.resolve();
    }
}

export { NavigoRouter }