import { AbstractPageInitializer } from '../../interfaces/irootbundle'

class SearchInitializer extends AbstractPageInitializer {

  async onStart() {
    await this.configureSearch();
  }

  async configureSearch() {
    let self = this;
    $(".search-btn").click(function (e) {
      $(".pranab").show();
      $(".search-bar").show();
      $(".site-nav").hide();

      self.updatePositionOfSearchResult();
      e.stopPropagation();
      e.preventDefault();
    });

    $(window).click(function (e) {
      if ($(e.target).closest('#mySearchContainer').length != 0) return;
      self.hideSearch();
    }).resize(function () {
      self.updatePositionOfSearchResult();
    });

    $("#mySearchContainer .search-result").on("click", "li", function (e) {
      let id = e.target.dataset["id"];
      let type = e.target.dataset["type"];
      switch (type) {
        case "2":
          self.hideSearch();
          self._route.navigate(`broker/${id}`);
          break;
        case "1":
          self.hideSearch();
          self._route.navigate(`news/${id}`);
          break;
        default:
      }
    });

    $(".pranab").on("keyup", self.searchResult.bind(self));
  }

  hideSearch() {
    $(".pranab").hide();
    $(".search-bar").hide();
    $(".site-nav").show();
  }

  updatePositionOfSearchResult() {
    let mySearch = $("#mySearch");
    $(".search-result")
      .css('left', mySearch.position().left)
      .width(mySearch.width() + 62);
  }

  lastFreezyTimerId: number = null;
  lastRequestId: number = null;

  searchResult(e) {
    let self = this;
    clearTimeout(self.lastFreezyTimerId);
    self.lastFreezyTimerId = setTimeout(function () {
      let queryWord = (<HTMLInputElement>e.target).value,
        requestId = self._random999999();

      if (!queryWord || !queryWord.trim()) { return; }
      queryWord = queryWord.trim()

      self.lastRequestId = requestId;
      $.ajax({
        method: "POST",
        url: "api/Search/Search",
        data: { query: queryWord }
      }).then((result) => {
        if (self.lastRequestId != requestId) { return; }

        let searchResultContainer = $(".search-result")
        if (!result || !result.length) {
          searchResultContainer[0].innerHTML = $("#search-no-found")[0].innerHTML;;
          return;
        }

        searchResultContainer[0].innerHTML = '';

        let ul = document.createElement("ul");

        for (let i = 0; i < result.length; i++) {
          let li = document.createElement("li");
          li.dataset["id"] = result[i]["id"];
          li.dataset["type"] = result[i]["type"];
          li.textContent = result[i]["foundedPart"];

          ul.appendChild(li);
        }
        searchResultContainer.append(ul);
      }).catch(function (err) {
        console.error("api/Search/search");
      });

    }, 1000);
  }
}

export { SearchInitializer };