import { QueryClientProvider } from "@tanstack/react-query";
import { getQueryClient } from "../lib/query-client";
import { AuthProvider } from "@/features/auth/context";

const queryClient = getQueryClient();

export function Provider({ children }) {
  return (
    <QueryClientProvider client={queryClient}>
      <AuthProvider>
        {children}
      </AuthProvider>
    </QueryClientProvider>
  );
}
