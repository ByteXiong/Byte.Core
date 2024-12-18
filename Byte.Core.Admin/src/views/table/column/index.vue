<script setup lang="tsx">
import type { DataTableColumn } from 'naive-ui';
import { NButton, NCheckbox, NInput, NPopconfirm, NSelect, useDialog } from 'naive-ui';
import { ref } from 'vue';
import { useForm, useRequest } from '@sa/alova/client';
import { useRoute, useRouter } from 'vue-router';
import { useDraggable } from 'vue-draggable-plus';
import { useAppStore } from '@/store/modules/app';
import { $t } from '@/locales';
import type { TableColumn, TableView } from '@/api/globals';
import { ColumnTypeEnum, OrderTypeEnum, SearchTypeEnum, ViewTypeEnum } from '@/api/apiEnums';
import AllGroupSelect from '@/components/select/all-group-select.vue';
import AllEnumSelect from '@/components/select/all-enum-select.vue';
import { getEnumValue } from '@/utils/common';
import { useFormRules, useNaiveForm } from '@/hooks/common/form';
import MonacoCode from '../modules/monaco-code.vue';
const route = useRoute();
const router = useRouter();
const configId = ref(route.query.configId as string);
const tableof = ref(route.query.tableof as string);
const viewType = ref<ViewTypeEnum>(route.query.viewType as unknown as number);
const dialog = useDialog();
const isDraggable = ref(true);
// #region 排序
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
// #endregion
// #region 提交视图(TableView)

type FormDataType = typeof tableView.value;

const { formRef, validate, restoreValidation } = useNaiveForm();
// 规则验证获取对象
const { defaultRequiredRule, patternRules } = useFormRules();
type RuleKey = keyof FormDataType;
const rules: Partial<Record<RuleKey, App.Global.FormRule | App.Global.FormRule[]>> = {
  sortKey: defaultRequiredRule,
  sortOrder: defaultRequiredRule
};

const {
  form: tableView,
  send: submitView,
  updateForm
} = useForm(
  from =>
    Apis.TableView.post_api_tableview_submit({
      data: from,
      transform: res => {
        window.$message?.success('保存成功！');
        // eslint-disable-next-line @typescript-eslint/no-use-before-define
        tableView.value.id = res.data;
      }
    }),
  {
    immediate: false,
    resetAfterSubmiting: false,
    initialForm: {
      sortKey: '',
      sortOrder: ''
    } as TableView,
    async middleware(_, next) {
      validate();
      await next();
    }
  }
);
// #endregion
const { loading, send: getTableHeader } = useRequest(
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
            onPositiveClick: () => {
              tableView.value.configId = configId.value;
              tableView.value.tableof = tableof.value;
              tableView.value.type = viewType.value;
              submitView();
            }
          });
        } else if (isDraggable.value) {
          updateForm(res.data || {});
          // 定时器10s后运行
          setTimeout(() => {
            const el = document.getElementsByTagName('tbody')[0];
            const { start } = useDraggable(el, tableView.value?.tableColumns, {
              animation: 150,
              // ghostClass: 'ghost',
              handle: '.handle',
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
// #region  行操作
const { send: submit, loading: submitLoading } = useForm(
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
        getTableHeader();
      }
    }),
  {
    force: true,
    immediate: false
  }
);

//= ===========================================设置头部=================================

//= ===========================================设置头部结束=================================

function renderColumnType(row: TableColumn) {
  switch (row.columnType) {
    case ColumnTypeEnum.枚举:
      return <AllEnumSelect v-model:value={row.columnTypeDetail} onUpdate:value={() => submit(row)} />;
    case ColumnTypeEnum.字典:
      return <AllGroupSelect v-model:value={row.columnTypeDetail} onUpdate:value={() => submit(row)} />;
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
const wrapperRef = ref<HTMLElement | null>(null);

const columns = ref<Array<DataTableColumn & { checked?: boolean }>>([
  {
    type: 'selection',
    align: 'center',
    width: 48,
    checked: true,
    disabled: row => row.id === '0'
  },
  {
    key: 'sort',
    title: '拖拽排序',
    align: 'center',
    checked: true,
    width: 80,
    render: row => {
      return (
        <div class="handle">
          <icon-mdi-drag class="mr-8px h-full cursor-move text-icon" />
          {row.sort}
        </div>
      );
    }
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
      return (
        <NInput
          type="text"
          v-model:value={row.title}
          placeholder="请输入注释"
          onChange={() => submit(row)}
          loading={submitLoading.value}
        />
      );
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
          disabled={submitLoading.value}
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
            loading={submitLoading.value}
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
          loading={submitLoading.value}
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
          loading={submitLoading.value}
          v-model:code={row.props}
          onChange={code => {
            row.props = code;
            submit(row);
          }}
        >
          {' '}
        </MonacoCode>
        {row.id !== '0' ? (
          <NPopconfirm onPositiveClick={() => handleDelete([row.id])}>
            {{
              default: () => $t('common.confirmDelete'),
              trigger: () => (
                <NButton type="error" ghost size="small" loading={submitLoading.value}>
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

function handleAdd() {
  tableView.value?.tableColumns?.push({
    viewId: tableView.value?.id
  });
}
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
          @refresh="getTableHeader"
        >
          <NButton size="small" ghost type="primary" @click="handleAdd()">
            <template #icon>
              <icon-ic-round-plus class="text-icon" />
            </template>
            {{ $t('common.add') }}
          </NButton>
          <!--       -->
          <NPopconfirm @positive-click="handleDelete(checkedRowKeys)">
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

      <NForm ref="formRef" :model="tableView" :rules="rules" label-placement="left" :label-width="80">
        <NGrid responsive="screen" item-responsive>
          <NFormItemGi span="24 s:12 m:6" label="默认排序" path="sortKey" class="pr-24px">
            <NSelect
              v-model:value="tableView.sortKey"
              :options="
                tableView?.tableColumns
                  ?.filter(x => !x.isCustom)
                  ?.map(item => ({ label: item.title || item.key, value: item.key }))
              "
              :loading="submitLoading"
              @update:value="submitView"
            ></NSelect>
            <NSelect
              v-model:value="tableView.sortOrder"
              :options="
                getEnumValue(OrderTypeEnum).map(item => ({ label: OrderTypeEnum[item], value: OrderTypeEnum[item] }))
              "
              placeholder="请选择"
              @update:value="submitView"
            ></NSelect>
          </NFormItemGi>
        </NGrid>
      </NForm>

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
/* .ghost {
  opacity: 0.5;
  background: #c8ebfb;
} */
</style>
