import { CookieService } from "./services/cookieService"
import { AbstractPageInitializer } from "./../interfaces/irootbundle";

import { SearchInitializer } from "./pageInitializer/searchInitializer";

class RootPageInitializer extends AbstractPageInitializer {
  async onStart() {
    var self = this;
    self.configureCulture();
    self.configureTradeDelegate();
    new SearchInitializer(self._route, null).onStart();

    $(window).scroll(function () {
      self.moveBanner();

      self.changeBannerClickableSquare();
      self.pressTheFooter();
      self.fixFooterProblem();
    });

    $(window).resize(() => {
      self.changeBannerClickableSquare();
      self.pressTheFooter();
      self.fixFooterProblem();
    });

    self.changeBannerClickableSquare();
    self.pressTheFooter();
  }

  private moveBanner() {
    let headerHeight = $(".site-header").height();
    let scrollTop = window.pageYOffset || document.documentElement.scrollTop;

    if (headerHeight < scrollTop) {
      $(".banner-wrapper-section").css("top", scrollTop - headerHeight);
    }
    else {
      $(".banner-wrapper-section").css("top", 0);
    }
  }

  private changeBannerClickableSquare() {
    let headerHeight = $(".site-header").height();

    let scrollTop = window.pageYOffset || document.documentElement.scrollTop;
    if (headerHeight > scrollTop) {
      $(".banner-image-wrapper").height(document.documentElement.clientHeight - headerHeight + scrollTop);
    } else {
      $(".banner-image-wrapper").height(document.documentElement.clientHeight);
    }
  }

  private pressTheFooter() {
    var siteBodyHeight = $(".site-body").height();
    var additionHeight = $("#after-page>.container").height();
    if (window.innerHeight > siteBodyHeight - additionHeight) {
      $("#after-page>.container").css("min-height", window.innerHeight - siteBodyHeight + additionHeight);
    } else {
      $("#after-page>.container").css("min-height", 0);
    }
  }

  private fixFooterProblem() {
    let scrollTop = window.pageYOffset || document.documentElement.scrollTop;

    let ft = $('footer');
    let ftTop = ft[0].getBoundingClientRect().top;
    let ftHeight = ft.height();
    let yOfEnd = ftTop + ftHeight + scrollTop;

    let scrollHeight = Math.max(
      document.body.scrollHeight, document.documentElement.scrollHeight,
      document.body.offsetHeight, document.documentElement.offsetHeight,
      document.body.clientHeight, document.documentElement.clientHeight
    );

    if (Math.round(yOfEnd) + 5 < scrollHeight) { // +5 Edge problem
      let innHeight = document.documentElement.clientHeight;
      window.scrollTo(0, yOfEnd - innHeight);
    }
  }

  private configureTradeDelegate() {
    $(document.body).on("click", ".trade-now-handler", function (e) {
      window.open((<any>window).urlToTrade, '_blank');

      e.stopImmediatePropagation();
      e.preventDefault();
    });
  }


  private configureCulture() {
    let cookieService = new CookieService();

    let culture = cookieService.getUserInterfaceCulture();

    this.setCultureInCookie(culture, cookieService);

    this.setLanguageImage(culture);

    $(".language-flag a").on("click", function (e) {
      let template = $("#language-select-template").html().trim();

      let parent = $(this).parent();

      parent.append(template);
      parent.find("select").val(culture || (<any>window).currentCultureFromServer);

      parent.find("select").change(function () {
        cookieService.setUserInterfaceCulture($(this).val());
        location.href = "/";
      });

      $(this).hide();

      return false;
    });

    $(window).on("click", function (e) {
      if ($(e.target).is(".language-flag")
        || $(e.target).is(".language-flag *")) {
        return;
      }

      let langSelector = $(".language-flag");
      langSelector.find("select").remove();
      langSelector.find("a").show();
    });
  }

  private setCultureInCookie(culture: string, cookieService: CookieService) {
    if (!culture && (<any>window).currentCultureFromServer) {
      cookieService.setUserInterfaceCulture((<any>window).currentCultureFromServer);
    }
  }

  private setLanguageImage(culture: string) {
    let fragment = $(document.createDocumentFragment())
      .append($("#language-select-template").html());
    let imageLink = fragment.find(`#language-selector option[value='${culture}']`).data('image');
    if (!imageLink) {
      imageLink = fragment.find(`#language-selector option[value='${(<any>window).currentCultureFromServer}']`).data('image');
    }
    if (!imageLink) {
      imageLink = fragment.find(`#language-selector option[value='en-US']`).data('image');
    }

    $(".icons.language-flag img").attr("src", imageLink);
  }
}

export { RootPageInitializer };