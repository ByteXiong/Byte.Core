export function getEnumValue(enumName: any): number[] {
  return Object.values<number>(enumName).filter((x) => typeof x === "number");
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
  App = 2,
}
export enum MenuTypeEnum {
  目录 = 1,
  菜单 = 2,
  按钮 = 3,
  外链 = 4,
}
export enum RoleTypeEnum {
  系统角色 = 10,
  公司角色 = 20,
  部门角色 = 30,
  个人角色 = 40,
}

export enum DeptTypeEnum {
  平台 = 10,

  公司 = 20,

  部门 = 30,
}
