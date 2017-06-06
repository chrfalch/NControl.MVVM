﻿using System;
using System.Collections.Generic;
using NControl.Mvvm;

namespace MvvmDemo
{
	public class FeedItem: BaseModel
	{
		public static IEnumerable<FeedItem> FeedRepository = new FeedItem[]{
			new FeedItem { Id = "5", City = "New York", Name = "Item 2" , Image = "https://images.pexels.com/photos/7613/pexels-photo.jpg?w=1260&h=750&auto=compress&cs=tinysrgb", Description = "New York is a state in the northeastern United States, and is the 27th-most extensive, fourth-most populous, and seventh-most densely populated U.S. state. New York is bordered by New Jersey and Pennsylvania to the south and Connecticut, Massachusetts, and Vermont to the east. The state has a maritime border in the Atlantic Ocean with Rhode Island, east of Long Island, as well as an international border with the Canadian provinces of Quebec to the north and Ontario to the northwest. The state of New York, with an estimated 19.8 million residents in 2015,[4] is also referred to as New York State to distinguish it from New York City, the state's most populous city and its economic hub."},
			new FeedItem { Id = "6", City = "New York", Name = "Item 3" , Image = "https://images.pexels.com/photos/8218/pexels-photo.jpg?w=1260&h=750&auto=compress&cs=tinysrgb" , Description = "New York is a state in the northeastern United States, and is the 27th-most extensive, fourth-most populous, and seventh-most densely populated U.S. state. New York is bordered by New Jersey and Pennsylvania to the south and Connecticut, Massachusetts, and Vermont to the east. The state has a maritime border in the Atlantic Ocean with Rhode Island, east of Long Island, as well as an international border with the Canadian provinces of Quebec to the north and Ontario to the northwest. The state of New York, with an estimated 19.8 million residents in 2015,[4] is also referred to as New York State to distinguish it from New York City, the state's most populous city and its economic hub."},
			new FeedItem { Id = "7", City = "New York", Name = "Item 1" , Image = "https://images.pexels.com/photos/12573/photo-1443453489887-98f56bc5bb38.jpeg?w=1260&h=750&auto=compress&cs=tinysrgb", Description = "New York is a state in the northeastern United States, and is the 27th-most extensive, fourth-most populous, and seventh-most densely populated U.S. state. New York is bordered by New Jersey and Pennsylvania to the south and Connecticut, Massachusetts, and Vermont to the east. The state has a maritime border in the Atlantic Ocean with Rhode Island, east of Long Island, as well as an international border with the Canadian provinces of Quebec to the north and Ontario to the northwest. The state of New York, with an estimated 19.8 million residents in 2015,[4] is also referred to as New York State to distinguish it from New York City, the state's most populous city and its economic hub."},
			new FeedItem { Id = "8", City = "New York", Name = "Item 2" , Image = "https://images.pexels.com/photos/8247/pexels-photo.jpg?w=1260&h=750&auto=compress&cs=tinysrgb" , Description = "New York is a state in the northeastern United States, and is the 27th-most extensive, fourth-most populous, and seventh-most densely populated U.S. state. New York is bordered by New Jersey and Pennsylvania to the south and Connecticut, Massachusetts, and Vermont to the east. The state has a maritime border in the Atlantic Ocean with Rhode Island, east of Long Island, as well as an international border with the Canadian provinces of Quebec to the north and Ontario to the northwest. The state of New York, with an estimated 19.8 million residents in 2015,[4] is also referred to as New York State to distinguish it from New York City, the state's most populous city and its economic hub."},
			new FeedItem { Id = "9", City = "New York", Name = "Item 3" , Image = "https://images.pexels.com/photos/7613/pexels-photo.jpg?w=1260&h=750&auto=compress&cs=tinysrgb" , Description = "New York is a state in the northeastern United States, and is the 27th-most extensive, fourth-most populous, and seventh-most densely populated U.S. state. New York is bordered by New Jersey and Pennsylvania to the south and Connecticut, Massachusetts, and Vermont to the east. The state has a maritime border in the Atlantic Ocean with Rhode Island, east of Long Island, as well as an international border with the Canadian provinces of Quebec to the north and Ontario to the northwest. The state of New York, with an estimated 19.8 million residents in 2015,[4] is also referred to as New York State to distinguish it from New York City, the state's most populous city and its economic hub."},
			new FeedItem { Id = "1", City = "New York", Name = "A skyline blah blah", Image = "https://images.pexels.com/photos/7613/pexels-photo.jpg?w=1260&h=750&auto=compress&cs=tinysrgb" , Description = "New York is a state in the northeastern United States, and is the 27th-most extensive, fourth-most populous, and seventh-most densely populated U.S. state. New York is bordered by New Jersey and Pennsylvania to the south and Connecticut, Massachusetts, and Vermont to the east. The state has a maritime border in the Atlantic Ocean with Rhode Island, east of Long Island, as well as an international border with the Canadian provinces of Quebec to the north and Ontario to the northwest. The state of New York, with an estimated 19.8 million residents in 2015,[4] is also referred to as New York State to distinguish it from New York City, the state's most populous city and its economic hub."},
			new FeedItem { Id = "2", City = "Tokyo", Name = "Item 2" , Image = "https://images.pexels.com/photos/1139/black-and-white-city-skyline-buildings.jpg?w=1260&h=750&auto=compress&cs=tinysrgb" , Description = "New York is a state in the northeastern United States, and is the 27th-most extensive, fourth-most populous, and seventh-most densely populated U.S. state. New York is bordered by New Jersey and Pennsylvania to the south and Connecticut, Massachusetts, and Vermont to the east. The state has a maritime border in the Atlantic Ocean with Rhode Island, east of Long Island, as well as an international border with the Canadian provinces of Quebec to the north and Ontario to the northwest. The state of New York, with an estimated 19.8 million residents in 2015,[4] is also referred to as New York State to distinguish it from New York City, the state's most populous city and its economic hub."},
			new FeedItem { Id = "3", City = "New York", Name = "Item 3" , Image = "https://images.pexels.com/photos/285011/pexels-photo-285011.jpeg?w=1260&h=750&auto=compress&cs=tinysrgb" , Description = "New York is a state in the northeastern United States, and is the 27th-most extensive, fourth-most populous, and seventh-most densely populated U.S. state. New York is bordered by New Jersey and Pennsylvania to the south and Connecticut, Massachusetts, and Vermont to the east. The state has a maritime border in the Atlantic Ocean with Rhode Island, east of Long Island, as well as an international border with the Canadian provinces of Quebec to the north and Ontario to the northwest. The state of New York, with an estimated 19.8 million residents in 2015,[4] is also referred to as New York State to distinguish it from New York City, the state's most populous city and its economic hub."},
			new FeedItem { Id = "4", City = "New York", Name = "Item 1" , Image = "https://images.pexels.com/photos/38203/pexels-photo-38203.jpeg?w=1260&h=750&auto=compress&cs=tinysrgb"} 
		};

		public FeedItem()
		{
		}

		public string Id
		{
			get { return GetValue<string>(); }
			set { SetValue(value); }
		}

		public string Image
		{
			get { return GetValue<string>(); }
			set { SetValue(value); }
		}

		public string Name
		{
			get { return GetValue<string>(); }
			set { SetValue(value); }
		}

		public string City
		{
			get { return GetValue<string>(); }
			set { SetValue(value); }
		}

		public string Description
		{
			get { return GetValue<string>(); }
			set { SetValue(value); }
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
