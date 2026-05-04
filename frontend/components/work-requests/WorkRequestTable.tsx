'use client';

import { WorkRequestSummary } from '@/types/workRequest';
import { StatusBadge, PriorityBadge } from '@/components/work-requests/StatusBadge';

interface WorkRequestTableProps {
  workRequests: WorkRequestSummary[];
  isLoading: boolean;
  onStatusUpdate: (id: string) => void;
}

// ── Date Formatter ────────────────────────────────────────────────

function formatDate(dateString: string): string {
  const date = new Date(dateString);
  return date.toLocaleDateString('en-GB', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
  });
}

// ── Loading Skeleton ──────────────────────────────────────────────

function SkeletonRow() {
  return (
    <tr className="animate-pulse border-b border-gray-800">
      {Array.from({ length: 7 }).map((_, i) => (
        <td key={i} className="px-4 py-4">
          <div className="h-4 rounded bg-gray-700/50" style={{ width: `${60 + Math.random() * 30}%` }} />
        </td>
      ))}
    </tr>
  );
}

// ── Table Component ───────────────────────────────────────────────

export default function WorkRequestTable({
  workRequests,
  isLoading,
  onStatusUpdate,
}: WorkRequestTableProps) {
  return (
    <div className="overflow-x-auto rounded-xl border border-gray-800 bg-gray-900/50">
      <table className="w-full text-left text-sm">
        <thead>
          <tr className="border-b border-gray-800 text-xs uppercase tracking-wider text-gray-400">
            <th className="px-4 py-3 font-medium">Title</th>
            <th className="px-4 py-3 font-medium">Client Name</th>
            <th className="px-4 py-3 font-medium">Priority</th>
            <th className="px-4 py-3 font-medium">Status</th>
            <th className="px-4 py-3 font-medium">Due Date</th>
            <th className="px-4 py-3 font-medium">Created</th>
            <th className="px-4 py-3 font-medium">Actions</th>
          </tr>
        </thead>
        <tbody>
          {/* Loading state */}
          {isLoading && (
            <>
              <SkeletonRow />
              <SkeletonRow />
              <SkeletonRow />
            </>
          )}

          {/* Empty state */}
          {!isLoading && workRequests.length === 0 && (
            <tr>
              <td colSpan={7} className="px-4 py-12 text-center text-gray-500">
                No work requests found.
              </td>
            </tr>
          )}

          {/* Data rows */}
          {!isLoading &&
            workRequests.map((wr) => (
              <tr
                key={wr.id}
                className="border-b border-gray-800/50 transition-colors hover:bg-gray-800/30"
              >
                <td className="px-4 py-3.5 font-medium text-gray-100">{wr.title}</td>
                <td className="px-4 py-3.5 text-gray-300">{wr.clientName}</td>
                <td className="px-4 py-3.5">
                  <PriorityBadge priority={wr.priority} />
                </td>
                <td className="px-4 py-3.5">
                  <StatusBadge status={wr.status} />
                </td>
                <td className="px-4 py-3.5 text-gray-300">{formatDate(wr.dueDate)}</td>
                <td className="px-4 py-3.5 text-gray-400">{formatDate(wr.createdAt)}</td>
                <td className="px-4 py-3.5">
                  <button
                    onClick={() => onStatusUpdate(wr.id)}
                    className="rounded-md border border-gray-700 px-3 py-1.5 text-xs font-medium text-gray-300 transition-colors hover:border-indigo-500 hover:text-indigo-400"
                  >
                    Update Status
                  </button>
                </td>
              </tr>
            ))}
        </tbody>
      </table>
    </div>
  );
}
