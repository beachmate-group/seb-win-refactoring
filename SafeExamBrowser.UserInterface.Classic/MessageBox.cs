﻿/*
 * Copyright (c) 2018 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Windows;
using SafeExamBrowser.Contracts.I18n;
using SafeExamBrowser.Contracts.UserInterface.MessageBox;
using SafeExamBrowser.Contracts.UserInterface.Windows;
using MessageBoxResult = SafeExamBrowser.Contracts.UserInterface.MessageBox.MessageBoxResult;

namespace SafeExamBrowser.UserInterface.Classic
{
	public class MessageBox : IMessageBox
	{
		private IText text;

		public MessageBox(IText text)
		{
			this.text = text;
		}

		public MessageBoxResult Show(string message, string title, MessageBoxAction action = MessageBoxAction.Confirm, MessageBoxIcon icon = MessageBoxIcon.Information, IWindow parent = null)
		{
			var result = default(System.Windows.MessageBoxResult);

			if (parent is Window window)
			{
				result = window.Dispatcher.Invoke(() => System.Windows.MessageBox.Show(window, message, title, ToButton(action), ToImage(icon)));
			}
			else
			{
				result = System.Windows.MessageBox.Show(message, title, ToButton(action), ToImage(icon));
			}

			return ToResult(result);
		}

		public MessageBoxResult Show(TextKey message, TextKey title, MessageBoxAction action = MessageBoxAction.Confirm, MessageBoxIcon icon = MessageBoxIcon.Information, IWindow parent = null)
		{
			return Show(text.Get(message), text.Get(title), action, icon, parent);
		}

		private MessageBoxButton ToButton(MessageBoxAction action)
		{
			switch (action)
			{
				case MessageBoxAction.YesNo:
					return MessageBoxButton.YesNo;
				default:
					return MessageBoxButton.OK;
			}
		}

		private MessageBoxImage ToImage(MessageBoxIcon icon)
		{
			switch (icon)
			{
				case MessageBoxIcon.Error:
					return MessageBoxImage.Error;
				case MessageBoxIcon.Question:
					return MessageBoxImage.Question;
				case MessageBoxIcon.Warning:
					return MessageBoxImage.Warning;
				default:
					return MessageBoxImage.Information;
			}
		}

		private MessageBoxResult ToResult(System.Windows.MessageBoxResult result)
		{
			switch (result)
			{
				case System.Windows.MessageBoxResult.Cancel:
					return MessageBoxResult.Cancel;
				case System.Windows.MessageBoxResult.No:
					return MessageBoxResult.No;
				case System.Windows.MessageBoxResult.OK:
					return MessageBoxResult.Ok;
				case System.Windows.MessageBoxResult.Yes:
					return MessageBoxResult.Yes;
				default:
					return MessageBoxResult.None;
			}
		}
	}
}