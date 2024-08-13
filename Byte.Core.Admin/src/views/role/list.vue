<template>
  <div class="list_main_content">
    <el-card shadow="never" class="table-container">
      <template #header>
        <el-input
          style="width: 150px; margin: 0px 5px"
          v-model="keyWord"
          placeholder="关键字查询"
          clearable
          @keyup.enter="search"
        />
      </template>
      <div class="flex justify-between mb-2">
        <div class="flex justify-around w-1/8">
          <el-tooltip effect="light" :content="'新增'" placement="top">
            <!-- <el-button type="primary"  icon="Plus"  circle @click="openForm()" >  </el-button> -->
            <el-icon @click="openForm()"><Plus /></el-icon>
          </el-tooltip>
          <el-tooltip effect="light" content="批量删除" placement="top">
            <el-icon @click="handleDelete(selectIds)"><Delete /></el-icon>
          </el-tooltip>

          <!-- <el-button type="danger" :disabled="selectIds.length==0"  @click="handleDelete(selectIds)">批量删除</el-button> -->
        </div>
        <div class="flex justify-around w-1/8">
          <el-tooltip effect="light" content="刷新" placement="top">
            <el-icon @click="reload"><Refresh /></el-icon>
          </el-tooltip>
          <el-tooltip effect="light" content="导出" placement="top">
            <el-icon><Download /></el-icon>
          </el-tooltip>

          <!-- <el-tooltip effect="light" content="设置表头" placement="top">
            <el-icon><Setting /></el-icon>
          </el-tooltip> -->
        </div>
        <!-- <el-button type="danger" icon="Delete" circle  :disabled="selectIds.length==0"  @click="handleDelete(selectIds)"/> -->

        <!-- <el-col :span="1" class="text-align-right">
            <excel tid="table_excel_WareHouse" :name="head.comment" />
          </el-col>
          <el-col :span="1" class="text-align-right">
            <head-seting v-model="head" name="WareHouseDTO" />
          </el-col> -->
      </div>
      <el-table
        v-loading="loading"
        highlight-current-row
        row-key="id"
        :data="data"
        :border="true"
        @selection-change="handleSelectionChange"
        @row-dblclick="
          (row: RoleDTO) => {
            openForm(row.id);
          }
        "
        @sort-change="handleSortChange"
        ref="tableRef"
      >
        <!--       @row-click="handleRowClick"
        :row-class-name="tableRowClassName" -->
        <el-table-column
          show-overflow-tooltip
          type="selection"
          width="55"
          class-name="onExcel"
        />

        <el-table-column
          show-overflow-tooltip
          prop="name"
          label="角色名称"
          width="150"
          sortable="custom"
        />
        <el-table-column
          show-overflow-tooltip
          prop="type"
          label="角色类型"
          width="150"
          sortable="custom"
        >
          <template #default="scope">{{
            RoleTypeEnum[scope.row?.type]
          }}</template>
        </el-table-column>
        <el-table-column
          show-overflow-tooltip
          prop="code"
          label="角色编码"
          width="150"
          sortable="custom"
        />
        <el-table-column show-overflow-tooltip prop="remark" label="备注" />
        <el-table-column show-overflow-tooltip label="状态" width="90">
          <template #default="scope">
            <el-switch
              v-model="scope.row.state"
              inline-prompt
              :loading="stateLoading"
              active-text="启用"
              inactive-text="禁用"
              @change="
                async (e: boolean) => {
                  setState(scope.row.id, e);
                }
              "
            />
          </template>
        </el-table-column>
        <el-table-column
          show-overflow-tooltip
          class-name="onExcel"
          label="操作"
          width="240"
        >
          <template #default="scope">
            <el-button
              :loading="loading"
              v-hasPerm="['role/update']"
              type="primary"
              link
              size="small"
              @click.stop="openForm(scope.row.id)"
              ><i-ep-edit />编辑</el-button
            >

            <el-button
              :loading="loading"
              v-hasPerm="['role/delete']"
              type="primary"
              link
              size="small"
              @click.stop="handleDelete([scope.row.id])"
              ><i-ep-delete />删除</el-button
            >
          </template>
        </el-table-column>

        <template #empty>
          <el-empty description="暂无数据" />
        </template>
      </el-table>
      <template #footer>
        <el-pagination
          :page-sizes="[10, 20, 50, 100]"
          layout="total, sizes, prev, pager, next, jumper"
          :total="total"
          v-model:current-page="page"
          v-model:page-size="pageSize"
        />
      </template>
      <edit-form ref="editform" @refresh="getData" />
    </el-card>
  </div>
</template>
<script lang="ts" setup>
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import editForm from "./editForm.vue";
import { RoleTypeEnum } from "@/api/apiEnums";
import "@/api";
import { RoleDTO } from "@/api/globals";
defineOptions({
  name: "Role",
  inheritAttrs: false,
});
const keyWord = ref("");
const sortList = ref<Record<string, string>>({ id: "asc" });
/**
 * 获取数据
 */
const {
  data,
  page,
  pageSize,
  total,
  loading,
  send: getData,
  reload,
} = usePagination(
  // Method实例获取函数，它将接收page和pageSize，并返回一个Method实例
  (page, pageSize) =>
    Apis.Role.get_api_role_getpage({
      params: {
        KeyWord: keyWord.value,
        pageIndex: page,
        pageSize: pageSize,
        sortList: sortList.value,
      },
    }),
  {
    watchingStates: [keyWord, sortList],
    // 请求前的初始数据（接口返回的数据格式）
    // initialData: {
    //   pagerInfo: {
    //     pageIndex: 1,
    //     pageSize: 10,
    //     totalRowCount: 0,
    //   },
    //   data: [],
    // },
    initialPage: 1, // 初始页码，默认为1
    initialPageSize: 10, // 初始每页数据条数，默认为10
    preloadPreviousPage: false, // 是否预加载下一页
    preloadNextPage: false, // 是否预加载上一页
    total: ({ data }) => data?.pagerInfo?.totalRowCount,
    data: ({ data }) => data?.data,
  }
);
/**
 * 删除
 */
const { send: delIds } = useRequest(
  (ids: string[]) =>
    Apis.Menu.delete_api_menu_delete({
      data: ids,
      transform: () => {
        ElMessage.success("删除成功");
        reload();
      },
    }),
  { immediate: false }
);
/**
 * 设置状态
 */
const { send: setState, loading: stateLoading } = useRequest(
  (id: string, state: boolean) =>
    Apis.Role.put_api_role_setstate({
      params: { id, state },
      transform: (res) => {
        ElMessage.success("状态更新成功");
        return res.data;
      },
    }),
  {
    immediate: false,
  }
);

async function search() {
  await getData();
}

const selectIds = ref<string[]>([]);
//多选
async function handleSelectionChange(e: any) {
  selectIds.value = e.map((x: any) => x.id);
}
//
const handleSortChange = (data: {
  column: any;
  prop: string;
  order: string;
}) => {
  sortList.value = { [data.prop]: data.order?.replace("ending", "") };
};

// 删除
async function handleDelete(ids: string[]) {
  if (ids.length == 0) {
    ElMessage.error("请勾选批量删除条目");
    return;
  }
  ElMessageBox.confirm("确定删除吗？", "删除", {
    confirmButtonText: "确认",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await delIds(ids);
    })
    .catch((error: any) => {
      console.log(error);
    });
}
//打开子页面
const editform = ref();
async function openForm(id?: string) {
  await editform.value.openForm(id);
}
</script>
