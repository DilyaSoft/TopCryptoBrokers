import "babel-polyfill";

import { NavigoRouter } from "./../services/navigoRouter"
import { CoinListPageInitializer } from "./pageInitializer/coinListPageInitializer"
import { FiatListPageInitializer } from "./pageInitializer/fiatListPageInitializer"

import { BrokerListPageInitializer } from "./pageInitializer/brokers/brokerListPageInitializer"
import { AddBrokerPageInitializer } from "./pageInitializer/brokers/addBrokerPageInitializer"
import { EditBrokerLocalizationPageInitializer } from "./pageInitializer/brokers/editBrokerLocalizationPageInitializer"
import { EditBrokerPageInitializer } from "./pageInitializer/brokers/editBrokerPageInitializer"

import { NewsListPageInitializer } from "./pageInitializer/news/newsListPageInitializer"
import { AddNewsPageInitializer } from "./pageInitializer/news/addNewsPageInitializer"
import { EditNewsPageInitializer } from "./pageInitializer/news/editNewsPageInitializer"
import { EditNewsLocalizationPageInitializer } from "./pageInitializer/news/editNewsLocalizationPageInitializer"

import { SettingsListTemplatesAndURlsPageInitializer }
  from "./pageInitializer/settings/SettingsListTemplatesAndURlsPageInitializer"
import { SettingsListTitlesPageInitializer } from "./pageInitializer/settings/settingsListTitlesPageInitializer"
import { EditSettingPageInitializer } from "./pageInitializer/settings/editSettingPageInitializer"

$(function () {
  let router = new NavigoRouter();

    router.addRoot('admin', BrokerListPageInitializer.bind(null, router, null));
    router.addRoot('admin/coinList', CoinListPageInitializer.bind(null, router, null));
    router.addRoot('admin/fiatList', FiatListPageInitializer.bind(null, router, null));

    router.addRoot('admin/addBroker', AddBrokerPageInitializer.bind(null, router, null));
    router.addRoot('admin/edditBroker/:id', EditBrokerPageInitializer.bind(null, router, null));
    router.addRoot('admin/editBrokerLocalization/:id/:culture',
    EditBrokerLocalizationPageInitializer.bind(null, router, null));
    router.addRoot('admin/brokerList', BrokerListPageInitializer.bind(null, router, null));

    router.addRoot('admin/newsList',
    NewsListPageInitializer.bind(null, router, null));
    router.addRoot('admin/addNews',
    AddNewsPageInitializer.bind(null, router, null));
    router.addRoot('admin/editNews/:id',
    EditNewsPageInitializer.bind(null, router, null));
    router.addRoot('admin/editNewsLocalization/:id/:culture',
    EditNewsLocalizationPageInitializer.bind(null, router, null));

    router.addRoot('admin/settingsTemplatesUrls', SettingsListTemplatesAndURlsPageInitializer.bind(null, router, null));
    router.addRoot('admin/settingsTitles', SettingsListTitlesPageInitializer.bind(null, router, null));
    router.addRoot('admin/editSetting/:idStr', EditSettingPageInitializer.bind(null, router, null));
    router.addRoot('admin/editSetting/:idStr/:query', EditSettingPageInitializer.bind(null, router, null));

    router.addSimpleRoot('', () => { router.navigate('admin/brokerList'); });

  router.resolve();
});

//Template Initializer

let userNamePromise = $.post(location.origin + "/adminRoot/GetUserInfo");
$(function () {
  userNamePromise.done(function (result) {
    let elem = $(".user-profile")[0];
    let textNode = document.createTextNode(result.value);
    elem.insertBefore(textNode, elem.firstChild);
  }).fail(function () {
    console.error("/adminRoot/GetUserInfo");
  });
});