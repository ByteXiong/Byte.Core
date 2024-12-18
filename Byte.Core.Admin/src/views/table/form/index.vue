<script setup lang="tsx">
import type { DataTableColumn } from 'naive-ui';
import { NButton, NCheckbox, NInput, NPopconfirm, NSelect, useDialog } from 'naive-ui';
import { ref } from 'vue';
import { useForm, useRequest } from '@sa/alova/client';
import { useRoute, useRouter } from 'vue-router';
import { useDraggable } from 'vue-draggable-plus';
import { useAppStore } from '@/store/modules/app';
import { $t } from '@/locales';
import '@/api';
import type { TableColumn } from '@/api/globals';
import { ColumnTypeEnum, SearchTypeEnum, ViewTypeEnum } from '@/api/apiEnums';
import AllGroupSelect from '@/components/select/all-group-select.vue';
import AllEnumSelect from '@/components/select/all-enum-select.vue';
import { getEnumValue } from '@/utils/common';
import MonacoCode from '../modules/monaco-code.vue';
const route = useRoute();
const router = useRouter();
const configId = ref(route.query.configId as string);
const tableof = ref(route.query.tableof as string);
const viewType = ref<number>(route.query.viewType as unknown as number);
const dialog = useDialog();
const isDraggable = ref(true);

const { send: submitSort } = useForm(
  (_, row) =>
    Apis.TableView.put_api_tableview_settablesort({
      data: row,
      transform: ({ data }) => {
        window.$message?.success('保存成功！');
        Object.assign(row, data);
      }
    }),
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: []
  }
);
// 创建视图
const { send: submitView } = useForm(
  (_, row) =>
    Apis.TableView.post_api_tableview_submit({
      data: row,
      transform: () => {
        window.$message?.success('保存成功！');
        // eslint-disable-next-line @typescript-eslint/no-use-before-define
        getData();
      }
    }),
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: []
  }
);

const {
  data: tableView,
  loading,
  send: getData
} = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  () =>
    Apis.TableView.get_api_tableview_gettableheader({
      params: {
        ConfigId: configId.value,
        Tableof: tableof.value,
        type: viewType.value
      },
      transform: res => {
        if (!res.success) {
          dialog.warning({
            title: `${ViewTypeEnum[viewType.value]}-${tableof.value}模型不存在`,
            content: () => '首次加载请创建模型',
            negativeText: '返回',
            positiveText: '确认',
            onNegativeClick: () => router.back(),
            onPositiveClick: () =>
              submitView({
                Tableof: tableof.value,
                type: viewType.value
              })
          });
        } else if (isDraggable.value) {
          // 定时器10s后运行
          setTimeout(() => {
            const el = document.getElementsByTagName('tbody')[0];
            const { start } = useDraggable(el, tableView.value?.tableColumns, {
              animation: 150,
              ghostClass: 'ghost',
              filter: '.no-drag',
              onUpdate() {
                tableView.value?.tableColumns?.forEach((item, index) => {
                  item.sort = index + 1;
                });

                submitSort(
                  tableView.value?.tableColumns
                    ?.filter(x => x.id)
                    ?.map(item => {
                      return { sort: item.sort, id: item.id };
                    })
                );
              }
            });
            start();
          }, 1000);
          isDraggable.value = false;
        }

        // start();
        return res.data || [];
      }
    }),
  {
    force: true,
    immediate: true
  }
);

const { send: submit } = useForm(
  (_, row) =>
    Apis.TableView.put_api_tableview_settableheader({
      data: row,
      transform: ({ data }) => {
        window.$message?.success('保存成功！');
        Object.assign(row, data);
      }
    }),
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: {} as TableColumn
  }
);

const { send: handleDelete } = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  ids =>
    Apis.TableView.delete_api_tableview_deletetableheader({
      data: ids,
      transform: () => {
        window.$message?.success('删除成功！');
        getData();
      }
    }),
  {
    force: true,
    immediate: false
  }
);
// 获取 class 属性

// const el = document.getElementsByTagName('tbody')[0];
// const { start } = useDraggable(el, tableView, {
//   animation: 150,
//   ghostClass: 'ghost',
//   onStart() {
//     console.log('start');
//   },
//   onUpdate() {
//     console.log('update');
//   }
// });
//= ===========================================设置头部=================================

//= ===========================================设置头部结束=================================

function renderColumnType(row: TableColumn) {
  switch (row.columnType) {
    case ColumnTypeEnum.枚举:
      return <AllEnumSelect v-model:value={row.columnTypeDetail} onChange={() => submit(row)} />;
    case ColumnTypeEnum.字典:
      return <AllGroupSelect v-model:value={row.columnTypeDetail} onChange={() => submit(row)} />;
    case ColumnTypeEnum.时间:
      return (
        <NInput
          type="text"
          v-model:value={row.columnTypeDetail}
          placeholder="请输入yyyy-MM-dd HH:mm:ss格式"
          onChange={() => submit(row)}
        />
      );
    case ColumnTypeEnum.单图:
    case ColumnTypeEnum.多图:
    case ColumnTypeEnum.文件:
      return (
        <NInput
          type="text"
          v-model:value={row.columnTypeDetail}
          placeholder="请输入图片前缀URL"
          onChange={() => submit(row)}
        />
      );
    default:
      return null;
  }
}

const appStore = useAppStore();
// const { bool: visible, setTrue: openModal } = useBoolean();

const wrapperRef = ref<HTMLElement | null>(null);

const columns = ref<Array<DataTableColumn & { checked?: boolean }>>([
  {
    type: 'selection',
    align: 'center',
    width: 48,
    checked: true,
    disabled: row => row.id === 0
  },
  {
    key: 'key',
    title: $t('字段'),
    align: 'center',
    checked: true,
    className: 'draggable'
  },
  {
    key: 'title',
    title: $t('注释'),
    align: 'center',
    checked: true,
    render: row => {
      return <NInput type="text" v-model:value={row.title} placeholder="请输入注释" onChange={() => submit(row)} />;
    }
  },
  {
    key: 'isShow',
    title: $t('是否显示'),
    align: 'center',
    checked: true,
    render: row => {
      return (
        <NCheckbox
          checked={row.isShow as boolean}
          checked-value={true}
          v-model:checked={row.isShow}
          onUpdate:checked={() => {
            submit(row);
          }}
        >
          {row.isShow ? '显示' : '隐藏'}
        </NCheckbox>
      );
    }
  },
  {
    key: 'columnType',
    title: $t('数据类型'),
    align: 'center',
    checked: true,
    render: row => {
      return (
        <div>
          <NSelect
            v-model:value={row.columnType}
            options={getEnumValue(ColumnTypeEnum).map(item => ({ label: ColumnTypeEnum[item], value: item }))}
            placeholder="请选择"
            clearable
            onChange={() => {
              submit(row);
            }}
          />
          {renderColumnType(row)}
        </div>
      );
    }
  },
  {
    key: 'searchType',
    title: $t('搜索类型'),
    align: 'center',
    checked: true,
    render: row => {
      return (
        <NSelect
          v-model:value={row.searchType}
          options={getEnumValue(SearchTypeEnum).map(item => ({ label: SearchTypeEnum[item], value: item }))}
          placeholder="请选择"
          clearable
          onChange={() => {
            submit(row);
          }}
        />
      );
    }
  },
  // {
  //   key: 'sort',
  //   title: $t('排序'),
  //   align: 'center',
  //   checked: true,
  //   render: row => {
  //     return (
  //       <NInput
  //         v-model:value={row.sort}
  //         placeholder="请选择"
  //         onChange={() => {
  //           submit(row);
  //         }}
  //       />
  //     );
  //   }
  // },
  {
    key: 'type',
    title: $t('操作'),
    align: 'center',
    checked: true,
    render: row => (
      <div class="flex-center gap-8px">
        <MonacoCode
          v-model:code={row.props}
          onChange={code => {
            row.props = code;
            submit(row);
          }}
        >
          {' '}
        </MonacoCode>
        {row.id !== 0 ? (
          <NPopconfirm onPositiveClick={() => handleDelete([row.id])}>
            {{
              default: () => $t('common.confirmDelete'),
              trigger: () => (
                <NButton type="error" ghost size="small">
                  {$t('common.delete')}
                </NButton>
              )
            }}
          </NPopconfirm>
        ) : null}
      </div>
    )
  }
]);
const checkedRowKeys = ref<string[]>([]);
// const { checkedRowKeys, onBatchDeleted, onDeleted } = useTableOperate(data, getData);

// const operateType = ref<OperateType>('add');

function handleAdd() {
  tableView.value?.tableColumns?.push({
    tableof: tableof.value,
    viewId: tableView.value?.id
  });
  // operateType.value = 'add';
  // openModal();
}
// function handleDelete(id: number) {
//   // request
//   // console.log(id);
//   // onDeleted();
// }

// /** the edit menu data or the parent menu data when adding a child menu */
// const editingData: Ref<Api.SystemManage.Menu | null> = ref(null);

// function handleEdit(item: Api.SystemManage.Menu) {
//   operateType.value = 'edit';
//   editingData.value = { ...item };

//   openModal();
// }

// function handleAddChildMenu(item: Api.SystemManage.Menu) {
//   operateType.value = 'addChild';

//   editingData.value = { ...item };

//   openModal();
// }

// const allPages = ref<string[]>([]);
// const columnChecks = ref<NaiveUI.TableColumnCheck[]>([
//   {
//     key: 'name',
//     title: $t('page.manage.menu.menuName'),
//     checked: true
//   }
// ]);
// async function getAllPages() {
//   const { data: pages } = await fetchGetAllPages();
//   allPages.value = pages || [];
// }
</script>

<template>
  <div ref="wrapperRef" class="flex-col-stretch gap-16px overflow-hidden lt-sm:overflow-auto">
    <NCard
      :title="$t(`设置-${ViewTypeEnum[viewType]}`)"
      :bordered="false"
      size="small"
      class="sm:flex-1-hidden card-wrapper"
    >
      <template #header-extra>
        <TableHeaderOperation
          id="TableHeader"
          v-model:columns="columns"
          tableof="TableHeaderDTO"
          :disabled-delete="checkedRowKeys.length === 0"
          :loading="loading"
          @refresh="getData"
        >
          <NButton size="small" ghost type="primary" @click="handleAdd()">
            <template #icon>
              <icon-ic-round-plus class="text-icon" />
            </template>
            {{ $t('common.add') }}
          </NButton>
          <!--       -->
          <NPopconfirm @positive-click="handleDelete">
            <template #trigger>
              <NButton size="small" ghost type="error" :disabled="checkedRowKeys?.length === 0">
                <template #icon>
                  <icon-ic-round-delete class="text-icon" />
                </template>
                {{ $t('common.batchDelete') }}
              </NButton>
            </template>
            {{ $t('common.confirmDelete') }}
          </NPopconfirm>
        </TableHeaderOperation>
      </template>

      <NSelect
        v-model:value="tableof"
        :options="tableView?.tableColumns?.map(item => ({ label: item.title, value: item.key }))"
      ></NSelect>

      <NDataTable
        v-model:checked-row-keys="checkedRowKeys"
        :columns="columns.filter(item => item.checked)"
        :data="tableView?.tableColumns || []"
        size="small"
        :flex-height="!appStore.isMobile"
        :scroll-x="702"
        :loading="loading"
        remote
        :row-key="row => row.id"
        class="sm:h-full"
      />
    </NCard>
  </div>
</template>

<style scoped>
.ghost {
  opacity: 0.5;
  background: #c8ebfb;
}
</style>
