# [1.6.5](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.6.4...v1.6.5) (2019-06-15)


### Bug Fixes

* protobuf is since 1.191 in it's own assembly ([1002a82](https://github.com/SiskSjet/SE_Mod_Utils/commit/1002a82))



# [1.6.4](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.6.3...v1.6.4) (2019-05-06)


### Bug Fixes

* fix missing message type check on entity messages ([4897365](https://github.com/SiskSjet/SE_Mod_Utils/commit/4897365))



# [1.6.3](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.6.2...v1.6.3) (2019-04-15)


### Features

* remove `Localization` because it's now possible to load resx files ([7c94c00](https://github.com/SiskSjet/SE_Mod_Utils/commit/7c94c00))



# [1.6.2](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.6.1...v1.6.2) (2019-02-25)


### Bug Fixes

* fix a crash when enter a message shorter than command prefix ([bccfd26](https://github.com/SiskSjet/SE_Mod_Utils/commit/bccfd26))



# [1.6.1](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.6.0...v1.6.1) (2019-01-30)


### Bug Fixes

* fix removed used using statement ([01ab9ce](https://github.com/SiskSjet/SE_Mod_Utils/commit/01ab9ce))



# [1.6.0](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.5.3...v1.6.0) (2019-01-30)


### Features

* add a CommandHandler ([9d0036c](https://github.com/SiskSjet/SE_Mod_Utils/commit/9d0036c))



<a name="1.5.3"></a>
# [1.5.3](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.5.2...v1.5.3) (2018-10-27)


### Bug Fixes

* fix an issue with custom actions names ([442747a](https://github.com/SiskSjet/SE_Mod_Utils/commit/442747a))



<a name="1.5.2"></a>
# [1.5.2](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.5.1...v1.5.2) (2018-10-25)


### Features

* **TerminalControls:** add extension method to create button actions ([d3ca28e](https://github.com/SiskSjet/SE_Mod_Utils/commit/d3ca28e))



<a name="1.5.1"></a>
# [1.5.1](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.4.0...v1.5.1) (2018-10-24)


### Features

* **TerminalControls:** add extension methods for terminal controls ([8e0d9c0](https://github.com/SiskSjet/SE_Mod_Utils/commit/8e0d9c0))



<a name="1.5.0"></a>
# [1.5.0](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.4.0...v1.5.0) (2018-10-24)


### Features

* **Localization:** add localization lib, old method only work offline ([8d86ec0](https://github.com/SiskSjet/SE_Mod_Utils/commit/8d86ec0))



<a name="1.4.1"></a>
# [1.4.1](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.4.0...v1.4.1) (2018-09-12)


### Bug Fixes

* fix a typo in an method name ([41d218](https://github.com/SiskSjet/SE_Mod_Utils/commit/41d218))



<a name="1.4.0"></a>
# [1.4.0](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.3.3...v1.4.0) (2018-09-04)


### Features

* **Localization:** removed library because there is a better way ([c99f548](https://github.com/SiskSjet/SE_Mod_Utils/commit/c99f548))
* **Logging:** add option to create a log entry on method enter/leave ([391dd9c](https://github.com/SiskSjet/SE_Mod_Utils/commit/391dd9c))


### BREAKING CHANGES

* **Localization:** Localization library removed



<a name="1.3.3"></a>
# [1.3.3](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.3.2...v1.3.3) (2018-08-12)


### Features

* **Net:** add a network handler ([d4b4d2b](https://github.com/SiskSjet/SE_Mod_Utils/commit/d4b4d2b))



<a name="1.3.2"></a>
# [1.3.2](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.3.1...v1.3.2) (2018-07-24)

### Bug Fixes

* **Profiler:** fix profiler thread safety ([3ec0d6](https://github.com/SiskSjet/SE_Mod_Utils/commit/3ec0d6))
* **Logging:** change lock object in default event handlers([2f8f89](https://github.com/SiskSjet/SE_Mod_Utils/commit/2f8f89))



<a name="1.3.1"></a>
# [1.3.1](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.3.0...v1.3.1) (2018-07-23)

### Bug Fixes

* **Profiler:** fix some formating issues with the profiler ([bb1b5eb](https://github.com/SiskSjet/SE_Mod_Utils/commit/bb1b5eb))



<a name="1.3.0"></a>
# [1.3.0](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.2.2...v1.3.0) (2018-07-23)

### Features

* **Profiler:** improve some formatting ([254960](https://github.com/SiskSjet/SE_Mod_Utils/commit/254960))



<a name="1.2.0"></a>
# [1.2.0](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.1.1...v1.2.0) (2018-07-23)

### Features

* **Profiler:** add an profiler ([6932dbf](https://github.com/SiskSjet/SE_Mod_Utils/commit/6932dbf))



<a name="1.1.1"></a>
# [1.1.1](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.1.0...v1.1.1) (2018-07-23)

### Bug Fixes

* **Logging:** fix timestamps to utc in and convert to local time on render. ([3b83ce](https://github.com/SiskSjet/SE_Mod_Utils/commit/3b83ce))



<a name="1.1.0"></a>
# [1.1.0](https://github.com/SiskSjet/SE_Mod_Utils/compare/v1.0.0...v1.1.0) (2018-07-23)

### Bug Fixes

* **Logging:** fix an issue with Logger used in multiple threads. ([985eb48](https://github.com/SiskSjet/SE_Mod_Utils/commit/985eb48))



<a name="1.0.0"></a>
# 1.0.0 (2018-06-06)

### Bug Fixes

* **Logging:** fix build config warning messages ([f2f391b](https://github.com/SiskSjet/SE_Mod_Utils/commit/f2f391b))
* **Logging:** implement static ForScope<TScope> method ([b1ef26c](https://github.com/SiskSjet/SE_Mod_Utils/commit/b1ef26c))
* **Logging:** remove Log.cs from project ([e5043d2](https://github.com/SiskSjet/SE_Mod_Utils/commit/e5043d2))


### Features

* **Localization:** add the ability to create an get localize strings ([6d8c2d0](https://github.com/SiskSjet/SE_Mod_Utils/commit/6d8c2d0))
* **Logging:** Add ablity to close ILogEventHandlers ([0d80d28](https://github.com/SiskSjet/SE_Mod_Utils/commit/0d80d28))
* **Logging:** Add Global- and LocalStorageHandler ([6d41fed](https://github.com/SiskSjet/SE_Mod_Utils/commit/6d41fed))
* **Logging:** add option to create a log entry on method enter/leave ([391dd9c](https://github.com/SiskSjet/SE_Mod_Utils/commit/391dd9c))


