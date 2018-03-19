﻿/*
 * Scroll View with Object Pool based on Unity UI Framework
 * Copyright (C) 2018 Joe Leung
 *
 * This program is free software: you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by the Free
 * Software Foundation, either version 3 of the License, or
 * any later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for
 * more details.
 *
 * You should have received a copy of the GNU Lesser General Public License along with
 * this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ListViewItemModel
{
    public System.Object Data { get { return data; } }

    System.Object data;

    public ListViewItemModel(System.Object data)
    {
        this.data = data;
    }
}
