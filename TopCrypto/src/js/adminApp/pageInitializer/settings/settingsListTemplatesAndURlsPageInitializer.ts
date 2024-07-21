import { AbstractSettingsListPageInitializer } from "./../../interfaces/abstractSettingsListPageInitializer"
import { SettingsListTableInitializer } from "./../../services/settingsListTableInitializer"

class SettingsListTemplatesAndURlsPageInitializer extends AbstractSettingsListPageInitializer {
  protected _urlToData = "/GetSettingsTemplatesAndURlsAdmin";
}

export { SettingsListTemplatesAndURlsPageInitializer }