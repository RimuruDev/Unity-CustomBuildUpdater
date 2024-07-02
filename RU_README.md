# Unity Custom Build Updater

Unity Custom Build Updater - это пакет для Unity, предназначенный для упрощения процесса сборки ваших проектов Unity. Он автоматизирует версионирование, архивирование и управление путями сборки, что делает управление и распространение сборок проще. Этот пакет особенно полезен для сборок WebGL.

## Возможности

- Автоматическое увеличение версии на основе типа сборки (Major, Feature, Bugfix, Build)
- Настраиваемые пути сборки с опциями по умолчанию и пользовательскими
- Автоматическое архивирование сборок в zip-файлы
- Простая инициализация и настройка через окно редактора Unity

## Установка

Чтобы установить пакет Unity Custom Build Updater, выполните следующие шаги:

1. Откройте ваш проект Unity.
2. Перейдите в `Window > Package Manager`.
3. Нажмите на кнопку `+` в верхнем левом углу.
4. Выберите `Add package from git URL...`.
5. Вставьте следующий URL: `https://github.com/RimuruDev/Unity-CustomBuildUpdater.git`
6. Нажмите `Add`.

## Инициализация

После установки пакета вам нужно инициализировать конфигурацию сборки. Выполните следующие шаги:

1. Перейдите в `RimuruDev Tools > Initialize BuildConfig` в верхнем меню.
2. Это создаст asset `BuildConfig` в `Assets/Resources/Editor/BuildConfig.asset`.

## Конфигурация

### BuildConfig

Asset `BuildConfig` содержит следующие настройки:

- **Company Name**: Название вашей компании.
- **Product Name**: Название вашего продукта.
- **Initial Version**: Начальная версия вашей сборки (например, `1.0.0.0`).
- **Build Path Type**: Тип пути сборки (`Default` или `Custom`).
- **Custom Build Path**: Пользовательский путь сборки, если `Build Path Type` установлен на `Custom`.
- **Archive Build**: Архивировать ли сборку в zip-файл.
- **Version Type**: Тип увеличения версии (`Major`, `Feature`, `Bugfix`, `Build`).
- **Version Pattern**: Шаблон для именования версии (например, `com.{company}.{product}.v{version}`).

### Пример

```csharp
using UnityEngine;

namespace RimuruDev.Unity_CustomBuildUpdater.CustomBuildUpdater.Editor
{
    [CreateAssetMenu(fileName = "BuildConfig", menuName = "Configs/Build/BuildConfig", order = 1)]
    public class BuildConfig : ScriptableObject
    {
        public string companyName = "AbyssMoth";
        public string productName = "SuperGame";
        public string initialVersion = "1.0.0.0";
        public BuildPathType buildPathType = BuildPathType.Default;
        public string customBuildPath = "Builds";
        public bool archiveBuild = true;
        public VersionType versionType = VersionType.Build;
        public string versionPattern = "com.{company}.{product}.v{version}";
    }
}
```

## Использование

### Обновление версии

Чтобы обновить текущую версию в конфигурации:

1. Откройте asset `BuildConfig`.
2. Нажмите на кнопку `Update Current Version`.

### Сборка проекта

Чтобы собрать проект с использованием настроек конфигурации:

1. Перейдите в `File > Build Settings`.
2. Настройте параметры сборки по необходимости (убедитесь, что выбрана сборка WebGL, если вы тестируете WebGL).
3. Нажмите `Build` и выберите папку для сборки.

## Вклад

Вклады приветствуются! Пожалуйста, не стесняйтесь отправлять Pull Request.

## Лицензия

Этот проект лицензирован по лицензии MIT.
