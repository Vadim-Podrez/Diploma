/*!
 * AdminLTE v4.0.0-beta3 (https://adminlte.io)
 * Copyright 2014-2025 Colorlib <https://colorlib.com>
 * Licensed under MIT (https://github.com/ColorlibHQ/AdminLTE/blob/master/LICENSE)
 */
(function (factory) {
	typeof define === 'function' && define.amd ? define(factory) :
	factory();
})((function () { 'use strict';

	Object.defineProperty(exports, "__esModule", { value: true });
	exports.FullScreen = exports.CardWidget = exports.DirectChat = exports.Treeview = exports.PushMenu = exports.Layout = void 0;
	const tslib_1 = require("tslib");
	const layout_1 = tslib_1.__importDefault(require("./layout"));
	exports.Layout = layout_1.default;
	const push_menu_1 = tslib_1.__importDefault(require("./push-menu"));
	exports.PushMenu = push_menu_1.default;
	const treeview_1 = tslib_1.__importDefault(require("./treeview"));
	exports.Treeview = treeview_1.default;
	const direct_chat_1 = tslib_1.__importDefault(require("./direct-chat"));
	exports.DirectChat = direct_chat_1.default;
	const card_widget_1 = tslib_1.__importDefault(require("./card-widget"));
	exports.CardWidget = card_widget_1.default;
	const fullscreen_1 = tslib_1.__importDefault(require("./fullscreen"));
	exports.FullScreen = fullscreen_1.default;

}));
//# sourceMappingURL=adminlte.js.map
