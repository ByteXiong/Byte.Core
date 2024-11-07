export function getEnumValue(enumName: any): number[] {
  return Object.values<number>(enumName).filter(x => typeof x === 'number');
}
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
export enum MenuTypeEnum {
  目录 = 1,
  菜单 = 2,
  按钮 = 3,
  外链 = 4
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
  等于 = 1,
  模糊 = 2,
  大于 = 3,
  小于 = 4,
  区间 = 5
}

export enum ColumnTypeEnum {
  整数 = 1,
  文本 = 2,
  枚举 = 3,
  字典 = 4,
  小数 = 5,
  时间 = 6,
  图片 = 7
}
