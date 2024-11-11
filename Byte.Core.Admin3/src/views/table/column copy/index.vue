<script setup lang="tsx">
import { NButton, NCheckbox, NInput, NPopconfirm, NSelect } from 'naive-ui';
import { ref } from 'vue';
import { useForm, useRequest } from '@sa/alova/client';
import { useRoute } from 'vue-router';
import { useAppStore } from '@/store/modules/app';
import { $t } from '@/locales';
import '@/api';
import type { TableColumn } from '@/api/globals';
import { ColumnTypeEnum, SearchTypeEnum, getEnumValue } from '@/api/apiEnums';
import AllGroupSelect from '@/components/select/all-group-select.vue';
import MonacoCode from './modules/monaco-code.vue';
const route = useRoute();
const tableof = ref(route.path.split('/').pop());
const {
  data: tableData,
  loading,
  send: getData
} = useRequest(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  () =>
    Apis.DataTable.get_api_datatable_gettableheader({
      params: {
        Table: tableof.value
      },
      transform: res => {
        return res.data.columns || [];
      }
    }),
  {
    force: true,
    immediate: true
  }
);


const { send: submit } = useForm(
  (_, row) =>
    Apis.DataTable.put_api_datatable_settableheader({
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
    Apis.DataTable.delete_api_datatable_deletetableheader({
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

function renderColumnType(row: TableColumn) {
  switch (row.columnType) {
    case ColumnTypeEnum.字典:
      return (
        <AllGroupSelect v-model:value={row.columnTypeDetail} placeholder="请输入字典" onChange={() => submit(row)} />
      );
    case ColumnTypeEnum.时间:
      return (
        <NInput
          type="text"
          v-model:value={row.columnTypeDetail}
          placeholder="请输入yyyy-MM-dd HH:mm:ss格式"
          onChange={() => submit(row)}
        />
      );
    case ColumnTypeEnum.图片:
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

const columns = ref<Array<NaiveUI.TableColumnCheck>>([
  {
    type: 'selection',
    align: 'center',
    width: 48,
    checked: true,
    disabled: row => row.id === 0
  },
  {
    key: 'table',
    title: $t('模型'),
    align: 'center',
    width: 80,
    checked: true
  },
  {
    key: 'key',
    title: $t('字段'),
    align: 'center',
    checked: true
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
          onUpdate:value={() => {
            submit(row);
          }}
        />
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
            onUpdate:value={() => {
              submit(row);
            }}
          />
          {renderColumnType(row)}
        </div>
      );
    }
  },
  {
    key: 'sort',
    title: $t('排序'),
    align: 'center',
    checked: true,
    render: row => {
      return (
        <NInput
          v-model:value={row.sort}
          placeholder="请选择"
          onChange={() => {
            submit(row);
          }}
        />
      );
    }
  },
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
  tableData.value?.push({
    table: tableof.value
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
    <NCard :title="$t('设置表头')" :bordered="false" size="small" class="sm:flex-1-hidden card-wrapper">
      <template #header-extra>
        <TableHeaderOperation
          v-model:columns="columns"
          tableof="TableHeaderDTO"
          :disabled-delete="checkedRowKeys.length === 0"
          :loading="loading"
          @add="handleAdd"
          @delete="handleDelete(checkedRowKeys)"
          @refresh="getData"
        />
      </template>
      <NDataTable
        v-model:checked-row-keys="checkedRowKeys"
        :columns="columns.filter(item => item.checked)"
        :data="tableData?.sort((a, b) => (a?.sort || 99) - (b?.sort || 99))"
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
