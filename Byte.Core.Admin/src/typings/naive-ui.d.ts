declare namespace NaiveUI {
  type ThemeColor = 'default' | 'error' | 'primary' | 'info' | 'success' | 'warning';
  type Align = 'stretch' | 'baseline' | 'start' | 'end' | 'center' | 'flex-end' | 'flex-start';

  type DataTableBaseColumn<T> = import('naive-ui').DataTableBaseColumn<T>;
  type DataTableExpandColumn<T> = import('naive-ui').DataTableExpandColumn<T>;
  type DataTableSelectionColumn<T> = import('naive-ui').DataTableSelectionColumn<T>;
  type TableColumnGroup<T> = import('naive-ui/es/data-table/src/interface').TableColumnGroup<T>;
  type PaginationProps = import('naive-ui').PaginationProps;
  type TableColumnCheck = import('@/api/globals').TableColumn & {
    checked: boolean;
  };
  type TableDataWithIndex<T> = import('@sa/hooks').TableDataWithIndex<T>;
  type FlatResponseData<T> = import('@sa/axios').FlatResponseData<T>;

  // 搜索
  type SearchParams = {
    [key: string]: {
      key: string;
      value: string;
      searchType?: import('@/api/apiEnums').SearchTypeEnum;
    };
  };

  // 配置
  type dataTableConfig = {
    sortList: Record<string, string>;
    // sortKey: string;
    // sortOrder: import('@/api/apiEnums').OrderTypeEnum;
  };

  /**
   * the custom column key
   *
   * if you want to add a custom column, you should add a key to this type
   */
  type CustomColumnKey = 'operate';

  type SetTableColumnKey<C, T> = Omit<C, 'key'> & { key: keyof T | CustomColumnKey };

  type TableData = Api.Common.CommonRecord<object>;

  type TableColumnWithKey<T> = SetTableColumnKey<DataTableBaseColumn<T>, T> | SetTableColumnKey<TableColumnGroup<T>, T>;

  type TableColumn<T> = TableColumnWithKey<T> | DataTableSelectionColumn<T> | DataTableExpandColumn<T>;

  type TableApiFn<T = any, R = Api.Common.CommonSearchParams> = (
    params: R
  ) => Promise<FlatResponseData<Api.Common.PaginatingQueryRecord<T>>>;

  /**
   * the type of table operation
   *
   * - add: add table item
   * - edit: edit table item
   */
  type TableOperateType = 'add' | 'edit';

  type GetTableData<A extends TableApiFn> = A extends TableApiFn<infer T> ? T : never;

  type NaiveTableConfig<A extends TableApiFn> = Pick<
    import('@sa/hooks').TableConfig<A, GetTableData<A>, TableColumn<TableDataWithIndex<GetTableData<A>>>>,
    'apiFn' | 'apiParams' | 'columns' | 'immediate'
  > & {
    /**
     * whether to display the total items count
     *
     * @default false
     */
    showTotal?: boolean;
  };
}
