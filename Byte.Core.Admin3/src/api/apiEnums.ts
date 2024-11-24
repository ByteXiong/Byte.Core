export enum VersionEnum {
  def = 0,
  /// <summary>
  /// Pc端
  /// </summary>
  Pc = 1,
  /// <summary>
  ///
  /// </summary>
  App = 2
}
export enum StateEnum {
  /// <summary>
  /// 删除
  /// </summary>
  del = -1,
  /// <summary>
  /// 正常
  /// </summary>
  normal = 0,
  /// <summary>
  /// 新增
  /// </summary>
  add = 1,
  /// <summary>
  /// 更新
  /// </summary>
  update = 2
}
export enum OrderType {
  /// <summary>
  /// 正序
  /// </summary>
  asc = 1,
  /// <summary>
  /// 倒序
  /// </summary>
  desc = 2
}

export enum ViewTypeEnum {
  主页 = 1,
  编辑页 = 2,
  详情页 = 3
}

export enum MenuTypeEnum {
  目录 = 1,
  菜单 = 2,
  按钮 = 3,
  参数 = 4
}
export enum RoleTypeEnum {
  系统角色 = 10,
  公司角色 = 20,
  部门角色 = 30,
  个人角色 = 40
}

export enum DeptTypeEnum {
  平台 = 10,

  公司 = 20,

  部门 = 30
}
/// <summary>
/// 对齐方式
/// </summary>
export enum TableAlignEnum {
  left = 1,
  center,
  right
}
/// <summary>
/// 固定方式
/// </summary>
export enum TableFixedEnum {
  left = 1,
  '' = 2,
  right = 3
}

export enum SearchTypeEnum {
  等于,
  模糊,
  大于,
  大于或等于,
  小于,
  小于或等于,
  区间
}

export enum ColumnTypeEnum {
  整数 = 1,
  文本 = 2,
  枚举 = 3,
  字典 = 4,
  小数 = 5,
  日期 = 6,
  时间 = 7,
  时间戳转当地日期 = 8,
  时间戳转当地时间 = 9,
  单图 = 10,
  多图 = 11,
  文件 = 12,
  布尔 = 13,
  颜色 = 14,
  自定义 = 99
}

export enum LayoutTypeEnum {
  Base = 1,
  Blank = 2
}
export enum IconTypeEnum {
  iconify图标 = 1,
  本地图标 = 2
}
