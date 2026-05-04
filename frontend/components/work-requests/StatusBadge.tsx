import { WorkRequestStatus, Priority } from '@/types/workRequest';

// ── Status Badge ──────────────────────────────────────────────────

const statusConfig: Record<WorkRequestStatus, { label: string; bg: string; text: string }> = {
  New: { label: 'New', bg: 'bg-blue-500/15', text: 'text-blue-400' },
  InProgress: { label: 'In Progress', bg: 'bg-amber-500/15', text: 'text-amber-400' },
  Blocked: { label: 'Blocked', bg: 'bg-red-500/15', text: 'text-red-400' },
  Completed: { label: 'Completed', bg: 'bg-emerald-500/15', text: 'text-emerald-400' },
};

export function StatusBadge({ status }: { status: WorkRequestStatus }) {
  const config = statusConfig[status] ?? statusConfig.New;
  return (
    <span className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-medium ${config.bg} ${config.text}`}>
      {config.label}
    </span>
  );
}

// ── Priority Badge ────────────────────────────────────────────────

const priorityConfig: Record<Priority, { label: string; bg: string; text: string }> = {
  Low: { label: 'Low', bg: 'bg-gray-500/15', text: 'text-gray-400' },
  Medium: { label: 'Medium', bg: 'bg-orange-500/15', text: 'text-orange-400' },
  High: { label: 'High', bg: 'bg-red-500/15', text: 'text-red-400' },
};

export function PriorityBadge({ priority }: { priority: Priority }) {
  const config = priorityConfig[priority] ?? priorityConfig.Low;
  return (
    <span className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-medium ${config.bg} ${config.text}`}>
      {config.label}
    </span>
  );
}
