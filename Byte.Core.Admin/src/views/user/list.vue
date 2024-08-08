<template>
  <div class="list_main_content">
    <el-row :gutter="20">
      <!-- 部门树 -->
      <el-col :lg="4" :xs="24">
        <el-card shadow="never">
          <el-tree
            default-expand-all
            :data="treeData"
            :props="{
              children: 'children',
              label: 'name',
            }"
            :expand-on-click-node="false"
            @node-click="handleNodeClick"
          />
        </el-card>
      </el-col>
      <el-col :lg="20" :xs="24">
        <el-card shadow="never">
          <el-table
            v-loading="loading"
            highlight-current-row
            row-key="id"
            :data="data"
            :border="true"
            @selection-change="handleSelectionChange"
            @row-dblclick="
              (row: any) => {
                openForm(row.id);
              }
            "
            ref="tableRef"
          >
            <el-table-column
              show-overflow-tooltip
              type="selection"
              width="55"
              class-name="onExcel"
            />
            <el-table-column show-overflow-tooltip label="Logo" width="80">
              <template #default="scope">
                <el-avatar :src="upuloadUrl + scope.row.avatar" />
              </template>
            </el-table-column>
            <el-table-column
              show-overflow-tooltip
              prop="account"
              label="账号"
              width="130"
            />
            <el-table-column
              show-overflow-tooltip
              prop="nickName"
              label="昵称"
              width="100"
            />
            <el-table-column
              show-overflow-tooltip
              prop="phone"
              label="电话号码"
              width="130"
            />
            <el-table-column
              show-overflow-tooltip
              prop="deptNames"
              label="组织"
              min-width="80"
            >
              <template #default="scope">
                <el-tag
                  type="warning"
                  v-for="(item, index) in scope.row.deptNames"
                  :key="index"
                  >{{ item }}</el-tag
                >
              </template>
            </el-table-column>

            <el-table-column
              show-overflow-tooltip
              prop="roleName"
              label="角色"
              width="100"
            />
            <el-table-column
              show-overflow-tooltip
              prop="userThirds"
              label="用户绑定"
              width="120"
            >
              <!-- <template #default="scope">
            <el-popover
              trigger="hover"
              placement="left"
              :width="200"
              v-if="scope.row.userThirds.length > 0"
            >
              <template #reference>
                <el-button v-hasPerm="['login/nobind']">解除绑定</el-button>
              </template>
              <el-tag
                @close="reBind(item)"
                closable
                type="success"
                v-for="item in scope.row.userThirds"
                :key="item.userId"
                >{{ UserThirdType[item.type] }}</el-tag
              >
            </el-popover></template
          > -->
            </el-table-column>
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
              width="150"
              fixed="right"
            >
              <template #default="scope">
                <el-button
                  :loading="loading"
                  v-hasPerm="['user/update']"
                  type="primary"
                  link
                  size="small"
                  @click.stop="openForm(scope.row.id)"
                  ><i-ep-edit />编辑</el-button
                >
                <el-button
                  :loading="loading"
                  v-hasPerm="['user/delete']"
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
              block
              v-model:current-page="page"
              v-model:page-size="pageSize"
            />
          </template>
          <edit-form ref="editform" @refresh="getData" />
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>
<script lang="ts" setup>
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import editForm from "./editForm.vue";
import { RoleTypeEnum } from "@/api/apiEnums";
import "@/api";
defineOptions({
  name: "User",
  inheritAttrs: false,
});
const upuloadUrl = import.meta.env.VITE_UPLOAD_PROXY_PREFIX;
/**
 * 获取 组织架构
 */
const { data: treeData } = useRequest(
  () => Apis.Dept.get_api_dept_gettree({ transform: ({ data }) => data }),
  {
    immediate: true,
  }
);
const deptId = ref("");
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
    Apis.User.get_api_user_getpage({
      params: {
        deptId: deptId.value,
        KeyWord: keyWord.value,
        pageIndex: page,
        pageSize: pageSize,
        sortList: sortList.value,
      },
    }),
  {
    watchingStates: [deptId, keyWord, sortList],
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
    Apis.User.delete_api_user_delete({
      data: ids,
      transform: (res) => {
        ElMessage.success("状态更新成功");
        return data;
      },
    }),
  { immediate: false }
);
/**
 * 设置状态
 */
const { send: setState, loading: stateLoading } = useRequest(
  (id: string, state: boolean) =>
    Apis.User.put_api_user_setstate({
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

async function handleNodeClick(data: any) {
  deptId.value = data?.id;
  await getData();
}

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
