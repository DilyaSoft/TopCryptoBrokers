import "babel-polyfill";
import "parsleyjs";

import { NavigoRouter } from "./../services/navigoRouter"
import { LoginPageInitializer } from "./pageInitializer/loginPageInitializer"

$(function () {
  //configureCulture();

  var router = new NavigoRouter();

  router.addRoot('*', LoginPageInitializer.bind(null, router, null));
  //router.addRoot('loginForm', LoginPageInitializer.bind(null, null));
  //router.addRoot('login/*', HomeInitializer.bind(null, null));

  router.resolve();
});