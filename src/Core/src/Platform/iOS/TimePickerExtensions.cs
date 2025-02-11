﻿using System;
using System.Globalization;
using Foundation;
using UIKit;

namespace Microsoft.Maui.Platform
{
	public static class TimePickerExtensions
	{
		public static void UpdateFormat(this MauiTimePicker mauiTimePicker, ITimePicker timePicker)
		{
			mauiTimePicker.UpdateTime(timePicker, null);
		}

		public static void UpdateFormat(this MauiTimePicker mauiTimePicker, ITimePicker timePicker, UIDatePicker? picker)
		{
			mauiTimePicker.UpdateTime(timePicker, picker);
		}

		public static void UpdateTime(this MauiTimePicker mauiTimePicker, ITimePicker timePicker)
		{
			mauiTimePicker.UpdateTime(timePicker, null);
		}

		public static void UpdateTime(this MauiTimePicker mauiTimePicker, ITimePicker timePicker, UIDatePicker? picker)
		{
			if (picker != null)
				picker.Date = new DateTime(1, 1, 1).Add(timePicker.Time).ToNSDate();

			var cultureInfo = Culture.CurrentCulture;

			if (string.IsNullOrEmpty(timePicker.Format))
			{
				NSLocale locale = new NSLocale(cultureInfo.TwoLetterISOLanguageName);

				if (picker != null)
					picker.Locale = locale;
			}

			var time = timePicker.Time;
			var format = timePicker.Format;

			mauiTimePicker.Text = time.ToFormattedString(format, cultureInfo);

			if (format != null)
			{
				if (format.IndexOf("H", StringComparison.Ordinal) != -1)
				{
					var ci = new CultureInfo("de-DE");
					NSLocale locale = new NSLocale(ci.TwoLetterISOLanguageName);

					if (picker != null)
						picker.Locale = locale;
				}
				else if (format.IndexOf("h", StringComparison.Ordinal) != -1)
				{
					var ci = new CultureInfo("en-US");
					NSLocale locale = new NSLocale(ci.TwoLetterISOLanguageName);

					if (picker != null)
						picker.Locale = locale;
				}
			}

			mauiTimePicker.UpdateCharacterSpacing(timePicker);
		}

		public static void UpdateTextAlignment(this MauiTimePicker textField, ITimePicker timePicker)
		{
			// TODO: Update TextAlignment based on the EffectiveFlowDirection property.
		}
	}
}