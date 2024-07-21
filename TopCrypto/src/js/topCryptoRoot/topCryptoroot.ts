import "babel-polyfill";

import { RootPageInitializer } from "./rootInitializer";

import { HomeInitializer } from "./pageInitializer/homeInitializer";
import { TopBrokersInitializer } from "./pageInitializer/topBrokersInitializer";
import { BrokerDetailInitializer } from "./pageInitializer/brokerDetailInitializer";
import { CalculatorInitializer } from "./pageInitializer/calculatorInitializer";
import { NewsDetailInitializer } from "./pageInitializer/newsDetailInitializer";
import { NavigoRouter } from "./../services/navigoRouter";
import { StaticPageInitializer } from "./pageInitializer/staticPageInitializer";


$(function () {

    let router = new NavigoRouter();   

    new RootPageInitializer(router, null).onStart();

 router.addRoot('home', HomeInitializer.bind(null, router, null));
 router.addRoot('broker/:id', BrokerDetailInitializer.bind(null, router, null));
 router.addRoot('brokers', TopBrokersInitializer.bind(null, router, null, "GetTopTemplate"));
 router.addRoot('brokers/page/:pageNumber', TopBrokersInitializer.bind(null, router, null, "GetTopTemplate"));
 router.addRoot('calculator', CalculatorInitializer.bind(null, router, null));
 router.addRoot('news/:id', NewsDetailInitializer.bind(null, router, null));
 router.addRoot('pages/:templateId', StaticPageInitializer.bind(null, router, null));
 router.addRoot('*', HomeInitializer.bind(null, router, null));   

  router.resolve();
});