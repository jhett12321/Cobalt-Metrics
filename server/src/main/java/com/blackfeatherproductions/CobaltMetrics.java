package com.blackfeatherproductions;

import io.vertx.core.AbstractVerticle;
import io.vertx.core.Vertx;
import io.vertx.core.logging.Logger;
import io.vertx.core.logging.LoggerFactory;
import java.util.function.Consumer;

/**
 * The main class for the metrics system. Contains accessors to various managers.
 * @author Jhett Black
 */
public class CobaltMetrics extends AbstractVerticle
{
    public static CobaltMetrics Instance;
    
    //Members
    private Logger logger;
       
    public static void main(String[] args)
    {
        Consumer<Vertx> runner = vertx -> {
            try
            {
                vertx.deployVerticle(CobaltMetrics.class.getName());
            }
            catch (Throwable t)
            {
                t.printStackTrace();
            }
        };
        
        Vertx vertx = Vertx.vertx();
        runner.accept(vertx);
    }
    
    @Override
    public void start()
    {
        Instance = this;
        
        logger = LoggerFactory.getLogger(io.vertx.core.logging.JULLogDelegateFactory.class);
        
        logger.info("Cobalt Metrics REST Service");
        logger.info("Starting up...");
    }
}
