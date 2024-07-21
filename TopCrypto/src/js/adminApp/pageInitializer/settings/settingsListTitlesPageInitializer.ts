import { AbstractSettingsListPageInitializer } from "./../../interfaces/abstractSettingsListPageInitializer"
import { SettingsListTableInitializer } from "./../../services/settingsListTableInitializer"

class SettingsListTitlesPageInitializer extends AbstractSettingsListPageInitializer {
  protected _urlToData = "/GetSettingsTitlesAdmin";
}

export { SettingsListTitlesPageInitializer }