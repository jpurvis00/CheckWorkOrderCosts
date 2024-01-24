
using DataAccessLibrary.Models;
using System.Text.RegularExpressions;

namespace DataAccessLibrary
{
    public class OracleCrud
    {
        private readonly string _connectionString;
        private OracleDataAccess _db = new OracleDataAccess();

        public OracleCrud(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ClosedWorkOrderModel> GetClosedWorkOrders(int dayToCheck)
        {
            string sql = $"select * from z_closed_jobs_nightly where close_date = TRUNC(SYSDATE - {dayToCheck})";

            return _db.LoadData<ClosedWorkOrderModel, dynamic>(sql, new { }, _connectionString);
        }

        public List<CostSummaryModel> GetCostSummaryActualMaterialCost(string workOrder)
        {
            string sql = $"select work_order_no, wo_qty, transaction_doc_type, total_mat_cost from v_wo_cost_ledger where work_order_no = '{workOrder}' and transaction_doc_type = 'WO_ISSUES'";

            return _db.LoadData<CostSummaryModel, dynamic>(sql, new { }, _connectionString);
        }

        public List<WorkOrderIssuesWCostModel> GetWOIssuesWithCost(string workOrder)
        {
            string sql = $"select work_order_no, wo_qty, transaction_doc_type, transaction_doc_no, transaction_doc_line_no, " +
                $"component_item_no, lot_no, total_mat_cost, total_labor_cost, total_foh_cost, total_sc_cost " +
                $"from v_wo_cost_ledger where work_order_no = '{workOrder}' and transaction_doc_type = 'WO_ISSUES'";

            return _db.LoadData<WorkOrderIssuesWCostModel, dynamic>(sql, new { }, _connectionString);
        }
       
        /* Replaced the following sql with GetTimeCardDetailsWithSetupTeardown.  This method was using a view that did not have
         * all the data I needed. Insteadn of using the view, I queried the actual time card details table. */
        //public List<TimeCardDetailsModel> GetTimeCardDetails(string workOrder)
        //{
        //    //string sql = $"select work_order_no, wo_qty, transaction_doc_type, transaction_doc_no, transaction_doc_line_no, " +
        //    //    $"employee_number, operation_code, work_center_no, good_qty, scrap_qty, qa_hold_qty, total_labor_cost, total_foh_cost " +
        //    //    $"from v_wo_cost_ledger where work_order_no = '{workOrder}' and transaction_doc_type = 'RUN_ACTUAL'";
        //    string sql = $"select work_order_no, wo_qty, transaction_doc_type, transaction_doc_no, transaction_doc_line_no, " +
        //        $"employee_number, operation_code, work_center_no, good_qty, scrap_qty, qa_hold_qty, total_labor_cost, total_foh_cost " +
        //        $"from v_wo_cost_ledger where work_order_no = '{workOrder}' and transaction_doc_type in ('RUN_ACTUAL', 'SETUP_ACTUAL')";

        //    return _db.LoadData<TimeCardDetailsModel, dynamic>(sql, new { }, _connectionString);
        //}

        /* Adding this so I can see the tear_down ops.  The view used for GetTimeCardDetails does not include that for some reason. */
        public List<TimeCardDetailsWithSetupTeardownModel> GetTimeCardDetailsWithSetupTeardown(string workOrder)
        {
            string sql = $"select work_order_no, tc_reporting_type, good_qty, scrap_qty, work_center_no, total_labor_cost, total_foh_cost " +
                $"from mfg_tc_detail where work_order_no = '{workOrder}'";

            return _db.LoadData<TimeCardDetailsWithSetupTeardownModel, dynamic>(sql, new { }, _connectionString);
        }

        public List<SubContractingModel> GetSubContractingDetails(string workOrder)
        {
            string sql = $"select work_order_no, wo_qty, transaction_doc_type, transaction_doc_no, transaction_doc_line_no, " +
                $"transaction_qty_buom, total_sc_cost from v_wo_cost_ledger where work_order_no = '{workOrder}' and transaction_doc_type = 'PO_RECEIPT'";

            return _db.LoadData<SubContractingModel, dynamic>(sql, new { }, _connectionString);
        }

        public List<WorkOrderCompletionModel> GetWorkOrderCompletionDetails(string workOrder)
        {
            string sql = $"select work_order_no, wo_qty, transaction_doc_type, transaction_doc_no, transaction_doc_line_no, " +
                $"transaction_qty_buom, lot_no, total_mat_cost, total_labor_cost, total_foh_cost, total_sc_cost " +
                $"from v_wo_cost_ledger where work_order_no = '{workOrder}' and transaction_doc_type = 'COMPLETION'";

            return _db.LoadData<WorkOrderCompletionModel, dynamic>(sql, new { }, _connectionString);
        }

        public List<IssuedMaterialCostsVsItemStandardCostsModel> GetIssuedMaterialCostsVsItemStandardCosts(string workOrder)
        {
            string sql = $"SELECT v.source_document_type, v.item_no, v.description, v.lot_no, v.transaction_qty, " +
                $"v.transaction_uom, v.work_order_no, v.unit_price as wo_issues_unit_price, v.unit_mat_cost as wo_issues_mat_cost, " +
                $"v.unit_labor_cost as wo_issues_labor_cost, v.unit_oh_cost as wo_issues_oh_cost, v.unit_sub_cntg_cost as wo_issues_sub_cntg_cost, " +
                $"c.item_no AS c_item_no, c.standard_cost as item_standard_cost, c.effective_from, c.effective_till, " +
                $"c.unit_mat_cost AS item_standard_mat_cost, c.unit_labor_cost as item_standard_labor_cost, " +
                $"c.unit_fixed_oh_cost as item_standard_fixed_oh_cost, c.unit_sub_cntg_cost as item_standard_sub_cntg_cost " +
                $"FROM v_wo_issues_ledger v " +
                $"cross apply( " +
                $"SELECT c_inner.* " +
                $"FROM im_item_cost c_inner " +
                $"WHERE c_inner.item_no = v.item_no " +
                $"ORDER BY c_inner.effective_till DESC " +
                $"FETCH FIRST 1 ROW ONLY" +
                $") c " +
                $"WHERE v.work_order_no = '{workOrder}'";

            return _db.LoadData<IssuedMaterialCostsVsItemStandardCostsModel, dynamic>(sql, new { }, _connectionString);
        }

    }
}
