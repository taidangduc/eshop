import { QueryClient } from "@tanstack/react-query";

export function getQueryClient() {
  return new QueryClient({
    defaultOptions: {
      queries: {
        staleTime: 60 * 1000,
        refetchOnWindowFocus: false,
        retry: (failureCount, error) => {
          if (
            error &&
            typeof error === "object" &&
            "status" in error &&
            error.status === 404
          )
            return false;
          return failureCount < 2;
        },
      },
    },
  });
}
